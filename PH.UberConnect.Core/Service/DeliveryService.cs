using PH.UberConnect.Api.EndpointsBody;
using PH.UberConnect.Api.EndpointsBody.DeliveryBodyProperties;
using PH.UberConnect.Api.Responses;
using PH.UberConnect.Api.Responses.Delivery;
using PH.UberConnect.Core.Interface;
using PH.UberConnect.Core.Models;
using PH.UberConnect.Webhook.Models.DeliveryStatusModels;
using System.Globalization;

namespace PH.UberConnect.Core.Service
{
    public class DeliveryService
    {
        private IRepositories Repositories { get; set; }

        public DeliveryService(IRepositories repositories)
        {
            Repositories = repositories;
        }

        /// <summary>
        /// Converts Facturas instances into a Deliveries instance
        /// </summary>
        /// <param name="locales">List of stores that Delivery needs to be completed</param>
        public DeliveryBody FromFactura(FacturaDireccion factura, Local local, List<FacturaProducto> facturaProductos, bool isTesting,
            List<ProductoEspecial> productoEspeciales)
        {
            var dropoffAdress = $"{factura.NombreProvincia}, {factura.NombreCanton}, {factura.NombreDistrito}, {factura.NombreBarrio}";
            var pickUpReadyDt = factura.FechaFacturado.AddMinutes(local.MinutosDT);
            var deliveryBody = new DeliveryBody(
                dropoff_name: factura.Nombre,
                dropoff_latitude: factura.Latitud,
                dropoff_longitude: factura.Longitud,
                dropoff_phone_number: factura.Telefono,
                dropoff_address: factura.NombreDireccion,
                pickup_name: local.Descripcion,
                pickup_address: local.Direccion,
                pickup_latitude: local.Latitud,
                pickup_longitude: local.Longitud,
                pickup_phone_number: $"+506{local.Telefono}",
                manifest_items: GetManifestItems(facturaProductos, productoEspeciales),
                external_store_id: local.CodRest?.ToString()
            )
            {
                dropoff_notes = $"{dropoffAdress}, {factura.Direccion}",
                manifest_reference = factura.IdFactura.ToString(),
                dropoff_verification = new VerificationRequirement(new SignatureRequirement(true, false, false), picture: false),
                pickup_verification = new VerificationRequirement(picture: true),
                manifest_total_value = Convert.ToInt32(factura.MontoTotal),
                pickup_ready_dt = pickUpReadyDt.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo),
                pickup_notes = local.DirecNota
            };

            if (isTesting)
                deliveryBody.test_specifications = new TestSpecifications { robo_courier_specification = new RoboCourierSpecification("auto") };

            return deliveryBody;
        }

        /// <summary>
        /// Gets the products of a delivery
        /// </summary>
        /// <param name="facturaProductos"></param>
        /// <returns></returns>
        public List<ManifestItem> GetManifestItems(List<FacturaProducto> facturaProductos, List<ProductoEspecial> productoEspeciales)
        {
            int firstLine = facturaProductos.FindIndex(prod => prod.NumLin == 1);
            facturaProductos.RemoveAt(firstLine);
            var manifestItems = new List<ManifestItem>();

            foreach (var producto in facturaProductos)
            {
                var size = "medium";
                var actualProductoEspecial = productoEspeciales.FirstOrDefault(proEsp => proEsp.CodPro == producto.CodPro);

                if (actualProductoEspecial != null)
                    size = GetItemSize(actualProductoEspecial.TipoTamaño);

                var manifestItem = new ManifestItem()
                {
                    name = producto.NomPro!.Trim(),
                    size = size,
                    quantity = int.Parse(producto.UniVen.ToString())
                };
                manifestItems.Add(manifestItem);
            }

            return manifestItems;
        }

        private string GetItemSize(int tipoTamaño)
        {
            return tipoTamaño switch
            {
                1 => "small",
                2 => "medium",
                3 => "large",
                4 => "xlarge",
                _ => "medium",
            };
        }

        /// <summary>
        /// Saves important data of a receipt after it sent to Uber.
        /// </summary>
        /// <param name="deliveryResponse"></param>
        /// <param name="factura"></param>
        /// <returns></returns>
        public int Insert(DeliveryResponse deliveryResponse, Factura factura, string jsonFactura)
        {
            return Repositories.DeliveryRepository.Insert(deliveryResponse, factura, jsonFactura);
        }

        /// <summary>
        /// Update data of the courier of a delivery
        /// </summary>
        public int UpdateCourier(Data data)
        {
            return Repositories.DeliveryRepository.UpdateCourier(data);
        }


        /// <summary>
        /// Updates pickup and dropoff dates of a delivery
        /// </summary>
        public int UpdateDateTimes(Data data, int idFactura)
        {
            return Repositories.DeliveryRepository.UpdateDateTimes(data, idFactura);
        }

        /// <summary>
        /// Gets the today's receipts that was sent to Uber.
        /// </summary>
        public List<Factura> GetToday()
        {
            return Repositories.DeliveryRepository.GetTodaysFactura();
        }

        public int UpdateCancellationReason(string reason, string personInCharge, string deliveryId)
        {
            return Repositories.DeliveryRepository.UpdateCancellationReason(reason, personInCharge, deliveryId);
        }
    }
}
