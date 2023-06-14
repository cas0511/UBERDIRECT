using Microsoft.AspNetCore.Mvc;
using PH.UberConnect.Core.Service;
using PH.UberConnect.Web.AppSettings;
using PH.UberConnect.Web.Models;
using Serilog;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PH.UberConnect.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly SigningKeys _signingKeys;
        private readonly Services _services;

        public DeliveryController(SigningKeys signingKeys, Services services)
        {
            _signingKeys = signingKeys;
            _services = services;
        }

        private string GetHMAC(string text, string key)
        {
            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                var hmac = BitConverter.ToString(hash).Replace("-", "").ToLower();
                return hmac;
            }

        }

        [HttpPost("/UpdateStatus")]
        public IActionResult UpdateStatus(object json)
        {
            try
            {
                if (json == null)
                    return BadRequest("Don't receive a response!");

                var externalSignature = Request.Headers["X-Postmates-Signature"].ToString();
                var secretKey = _signingKeys.DeliveryUpdateEvent;
                var internalSignature = GetHMAC(json.ToString(), secretKey);

                if (externalSignature != internalSignature)
                {
                    Log.Warning("The keys don't match!");
                    return BadRequest("The keys don't match!");
                }

                Log.Debug("Delivery: {delivery}", json.ToString());
                var deliveryStatus = JsonSerializer.Deserialize<DeliveryStatus>(json!.ToString());

                if (deliveryStatus == null)
                    return BadRequest("Error in deserialise the response into [DeliveryStatus] object");

                var factura = _services.FacturaService.GetById(deliveryStatus!.data.manifest.reference);

                if (factura == null) return BadRequest("Cannot find the factura");

                var currentLocal = _services.LocalesService.Get(factura!.IdRestauranteCliente);
                var uberStatus = _services.DeliveryHistoryService.GetEventStatus(deliveryStatus!.status, deliveryStatus.data.uuid[^5..].ToUpper());

                // Actualiza el estado de la factura
                var rowsAffected = _services.FacturaService.UpdateState(factura.IdFactura, uberStatus.StatusCode);
                Log.Information("Estado de la factura actualizada. {row} fila(s) afectada(s)", rowsAffected);

                // Registra el movimiento de la entrega
                rowsAffected = _services.DeliveryHistoryService.Insert(factura, uberStatus, json.ToString());
                Log.Information("Se registra el movimiento del pedido de la factura #{fac}. {row} fila(s) afectada(s)", factura.IdFactura, rowsAffected);

                // Actualiza las fechas del pickup and dropoff
                rowsAffected = _services.DeliveryService.UpdateDateTimes(deliveryStatus.data, factura.IdFactura);
                Log.Information("Se actualiza las fechas de la factura #{fac}. {row} fila(s) afectada(s)", factura.IdFactura, rowsAffected);

                // Guarda información del motorizado
                if (deliveryStatus.data.courier != null)
                {
                    rowsAffected = _services.DeliveryService.UpdateCourier(deliveryStatus.data);
                    Log.Information("Se guarda los datos del motorizado en la fac# {fac}", factura.IdFactura);
                }

                // Actualiza area de la factura en la tienda por estado
                rowsAffected = _services.LocalesService.Update(factura.NumFacturaCall, currentLocal, uberStatus.Area);
                Log.Information("Area de la factura actualizada en la tienda. {row} fila(s) afectada(s)", rowsAffected);

                if (uberStatus.StatusCode == 5 && currentLocal.ActulTiempo)
                {
                    var tiempo = _services.LocalesService.GetTiempos(factura.NumFacturaCall, currentLocal);
                    var closedDate = DateTime.Now;
                    rowsAffected = _services.LocalesService.CloseOrder(factura.NumFacturaCall, currentLocal, closedDate, tiempo);
                    Log.Information("La factura #{fac} cerrada. {row} fila(s) afectada(s)", factura.IdFactura, rowsAffected);
                }

                return Ok("Delivery information uploaded successfully!");
            }
            catch (Exception ex)
            {
                Log.Error("Ocurrió el siguiente error: {ex}", ex.Message);
                return BadRequest($"Sorry, an error has ocurred! {ex.Message}");
            }
        }
    }
}
