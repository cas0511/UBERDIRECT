using PH.UberConnect.Api;
using PH.UberConnect.Api.EndpointsBody;
using PH.UberConnect.Api.Responses;
using PH.UberConnect.Core.Exceptions;
using PH.UberConnect.Core.Models;
using PH.UberConnect.Core.Models.Status;
using PH.UberConnect.Core.Service;
using PH.UberConnect.Settings;
using Serilog;
using System.Collections;
using System.Diagnostics;

namespace PH.UberConnect
{
    public class Worker : BackgroundService
    {
        private readonly ApiSettings _apiSettings;
        private readonly Apis _apis;
        private readonly Services _services;
        private readonly bool _isTesting = false;

        public Worker(ApiSettings apiSettings, Apis api, Services services)
        {
            _apiSettings = apiSettings;
            _apis = api;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var authEndpointBody = new AuthenticationBody(_apiSettings.ClientId,
                _apiSettings.ClientSecret,
                _apiSettings.GrantType,
                _apiSettings.Scope);

            var authEndpointParams = new EndpointParams(body: authEndpointBody);
            var authResponse = _apis.Authentication.Execute<AuthenticationResponse>(authEndpointParams);

            if (authResponse == null)
            {
                Log.Warning("Hubo un problema en obtener el token");
                return;
            }

            var expiresDate = DateTime.Now.AddSeconds(authResponse.expires_in);
            Log.Information("El token se renoverá el {date}", expiresDate.ToString("dd-MM-yyyy hh:mm tt"));

            List<Local> localList;
            List<ProductoEspecial> productoEspecialList;

            try
            {
                productoEspecialList = _services.ItemSizeService.Get();
            }
            catch (Exception ex)
            {
                Log.Error("Error en obtener las tiendas o los productos especiales. {ex}", ex.Message);
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var todayDate = DateTime.Now;

                if (todayDate > expiresDate.AddDays(-1))
                {
                    authResponse = _apis.Authentication.Execute<AuthenticationResponse>(authEndpointParams);
                    expiresDate = todayDate.AddSeconds(authResponse!.expires_in);
                    Log.Information("Se renueva el token de Uber Direct, se renovará el {date}", expiresDate.ToString("dd-MM-yyyy hh:mm zz"));
                }

                try
                {
                    localList = _services.LocalesService.Get();
                    var localIdList = GetLocalIdList(localList);
                    var facturas = _services.FacturaService.Get(localIdList);
                    var facturasHistoryList = _services.DeliveryHistoryService.Get();
                    var facturasByLocal = _services.FacturaService.GetFacturasGroupByLocales(facturas, localList, facturasHistoryList);
                    var tasks = new List<Task>();
                    Log.Information("{count} factura(s) obtenidas", facturas.Count);

                    foreach (KeyValuePair<Local, List<FacturaDireccion>> local in facturasByLocal)
                    {
                        Log.Information("{count} factura(s) obtenidas de la tienda {store}", local.Value.Count, local.Key.Descripcion);
                        var task = Task.Factory.StartNew(() =>
                            SendFacturaList(local, authResponse.access_token, productoEspecialList)
                        , stoppingToken);
                        tasks.Add(task);

                        if (tasks.Count >= 10)
                        {
                            Task.WaitAll(tasks.ToArray(), stoppingToken);
                        }
                    }

                    Task.WaitAll(tasks.ToArray(), stoppingToken);
                    stopwatch.Stop();
                    Log.Information($"Worker terminó en {stopwatch.ElapsedMilliseconds / 1000.0} segundos");
                }
                catch (Exception ex)
                {
                    Log.Error("Error: {error}", ex.Message);
                    Log.Error("Ubicación del error: {stackTrace}", ex.StackTrace);
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        public void SendFacturaList(KeyValuePair<Local, List<FacturaDireccion>> local, string token, List<ProductoEspecial> productoEspecialList)
        {
            var deliveryIdList = new Dictionary<string, Factura>();

            foreach (var factura in local.Value)
            {
                var deliveryId = SendFactura(factura, local.Key, token, productoEspecialList);

                if (!string.IsNullOrEmpty(deliveryId))
                    deliveryIdList.Add(deliveryId, factura);
            }

            try
            {
                // Updates the area of a list of receipts by restaurant
                var rowsAffected = _services.LocalesService.UpdateIn(deliveryIdList, local.Key);
                Log.Information("Areas de las facturas #{facs} actualizadas en {store}. {rows} afectadas", ShowList(deliveryIdList), local.Key.Descripcion,
                    rowsAffected);

            }
            catch (Exception ex)
            {
                SaveErrors(local.Value, ex);
            }
        }

        /// <summary>
        /// Sends Factura to Uber API as a delivery, it returns the DeliveryId if it was sent successfully, otherwise returns string.empty
        /// </summary>
        /// <returns></returns>
        public string SendFactura(FacturaDireccion factura, Local local, string token, List<ProductoEspecial> productoEspecialList)
        {
            try
            {
                var rowsAffected = 0;
                var productos = _services.FacturaProductoService.Get(factura.IdFactura);
                var delivery = _services.DeliveryService.FromFactura(factura, local, productos, _isTesting, productoEspecialList);
                Log.Debug("Entrega: {delivery}", delivery.ToJson());

                // Sends the receipt to Uber Connect
                var data = new EndpointParams(token, delivery);
                var response = _apis.CreateDelivery.Execute<DeliveryResponse>(data);
                Log.Information("Factura #{factura} enviada a Uber Connect id {id}", factura.IdFactura, response?.id ?? "NO ID");

                // Updates the factura's estatus UD after send it to Uber Connect
                rowsAffected = _services.FacturaService.UpdateState(factura.IdFactura, 1);
                Log.Information("Estatus UD de la fac# {factura} actualizada. {rows} fila(s) afectada(s)", factura.IdFactura, rowsAffected);

                // Inserts the uber's delivery into a table in the DB ClickEat
                rowsAffected = _services.DeliveryService.Insert(response, factura, delivery.ToJson());
                Log.Information("Entrega #{idEntrega} registrada con la fac# {facId}. {rows} fila(s) afectada(s)",
                    response.id, factura.IdFactura, rowsAffected);

                return response.uuid!;
            }
            catch (RestSharpRequestException restSharpEx)
            {
                var extraInfo = string.Empty;

                foreach (DictionaryEntry de in restSharpEx.Data)
                    extraInfo += $"\nKey: {de.Key}  Value: {de.Value}";

                Log.Error("Error en la fac# {factura}: {error}{extraInfo}", factura.IdFactura, restSharpEx.Message, extraInfo);
                var errorMessage = $"Error en la fac# {factura.IdFactura}: {restSharpEx.Message}{extraInfo}";
                var statusCode = new ErrorStatus(errorMessage, -1, factura.IdFactura.ToString());
                SaveError(factura, statusCode);
                return string.Empty;

            }
            catch (Exception ex)
            {
                Log.Error("Error en la fac# {factura}: {error}\nUbicación del error: {stackTrace}", factura.IdFactura, ex.Message, ex.StackTrace);
                var statusCode = new ErrorStatus(ex.Message, -2, factura.IdFactura.ToString());
                SaveError(factura, statusCode);
                return string.Empty;
            }
        }

        public void SaveErrors(List<FacturaDireccion> facturaList, Exception ex)
        {
            foreach (var factura in facturaList)
            {
                Log.Error("Error en la fac# {factura}: {error}\nUbicación del error: {stackTrace}", factura.IdFactura, ex.Message, ex.StackTrace);
                var statusCode = new ErrorStatus(ex.Message, -2, factura.IdFactura.ToString());
                SaveError(factura, statusCode);
            }
        }

        /// <summary>
        /// Saves an error that it occurs in the worker into a database
        /// </summary>
        public void SaveError(Factura factura, UberStatus uberStatus)
        {
            try
            {
                _services.DeliveryHistoryService.Insert(factura, uberStatus, string.Empty);
                Log.Information("Error de la fac# {facId} registrada con éxito", factura.IdFactura);
            }
            catch (Exception ex)
            {
                Log.Error("¡Error en registrar el error: {ex}!", ex.Message);
            }
        }

        private string ShowList(Dictionary<string, Factura> facturasSent)
        {
            var finalString = "";
            var firstString = true;

            foreach (KeyValuePair<string, Factura> item in facturasSent)
            {
                if (firstString)
                {
                    finalString += item.Value.IdFactura;
                    firstString = false;
                }
                else
                {
                    finalString += $", #{item.Value.IdFactura}";
                }
            }

            return finalString;
        }

        /// <summary>
        /// Returns a list of string with the ids of the stores received.
        /// </summary>
        /// <param name="locals"></param>
        /// <returns></returns>
        private List<string> GetLocalIdList(List<Local> locals)
        {
            var localIdList = new List<string>();

            foreach (var local in locals)
            {
                localIdList.Add(local.CodRest);
            }

            return localIdList;
        }
    }
}