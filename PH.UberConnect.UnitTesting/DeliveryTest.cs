using PH.UberConnect.Api.EndpointsBody;
using PH.UberConnect.Api.EndpointsBody.DeliveryBodyProperties;
using PH.UberConnect.Api.Responses.Delivery;
using PH.UberConnect.Core.Models;
using PH.UberConnect.Core.Repositories;
using PH.UberConnect.Core.Service;

namespace PH.UberConnect.UnitTesting
{
    [TestClass]
    public class DeliveryTest
    {
        private DeliveryService DeliveryService = new(repositories: new Repositories(""));

        private List<FacturaProducto> BuildFacturaProductos()
        {
            return new List<FacturaProducto>
            {
                new FacturaProducto { NumLin = 1, CodPro = "00999", NomPro = "<Num Ord>", UniVen = 1 },
                new FacturaProducto { NumLin = 2, CodPro = "94213", NomPro = "AK7", UniVen = 2 },
                new FacturaProducto { NumLin = 3, CodPro = "24030", NomPro = "Promo-Pizza Personal", UniVen = 2 },
                new FacturaProducto { NumLin = 4, CodPro = "24030", NomPro = "Promo-Pizza Personal", UniVen = 2 },
                new FacturaProducto { NumLin = 5, CodPro = "24030", NomPro = "Promo-Pizza Personal", UniVen = 2 },
                // Add more sample data as needed
            };
        }

        private List<ProductoEspecial> BuildProductosEspeciales()
        {
            return new List<ProductoEspecial>
            {
                new ProductoEspecial { CodPro = "94213", TipoTamaño = 3 },
                // Add more sample data as needed
            };
        }

        [TestMethod]
        public void GetManifestItems_ShouldReturnCorrectManifestItems()
        {
            // Arrange
            var facturaProductos = BuildFacturaProductos();

            var productoEspeciales = BuildProductosEspeciales();

            var expectedManifestItems = new List<ManifestItem>
            {
                new ManifestItem { name = "AK7", size = "large", quantity = 2 },
                new ManifestItem { name = "Promo-Pizza Personal", size = "medium", quantity = 2 },
                new ManifestItem { name = "Promo-Pizza Personal", size = "medium", quantity = 2 },
                new ManifestItem { name = "Promo-Pizza Personal", size = "medium", quantity = 2 },
                // Add more expected ManifestItem objects as needed
            };

            // Act
            var manifestItems = DeliveryService.GetManifestItems(facturaProductos, productoEspeciales);

            // Assert
            CollectionAssert.AreEquivalent(expectedManifestItems, manifestItems);
        }

        [TestMethod]
        public void FromFactura_ReturnsDeliveryBody()
        {
            // Arrange
            var factura = new FacturaDireccion
            {
                NombreProvincia = "SAN JOSE",
                NombreCanton = "SANTA ANA",
                NombreDistrito = "POZOS",
                NombreBarrio = "GAVILANES",
                Latitud = 9.94419025278417,
                Longitud = -84.1809961,
                NombreDireccion = "Condominio hacienda las palmas",
                Direccion = "200 al este de alimentos la soya",
                PuntoReferencia = "Condominio hacienda las palmas casa 31",
                Telefono = "+50689111336",
                MontoTotal = 13765,
                IdFactura = 7842157,
                FechaFacturado = new DateTime(2023, 5, 7, 21, 7, 8, 7)
            };

            var local = new Local
            {
                CodRest = "45",
                Descripcion = "RIO ORO",
                Direccion = "250 metros este del Colegio de Santa Ana",
                Latitud = 9.9335899353027344,
                Longitud = -84.19000244140625,
                Telefono = "47005845",
                TiempoEntrega = 30,
                MinutosDT = 8
            };

            var facturaProductos = BuildFacturaProductos();

            var productoEspeciales = BuildProductosEspeciales();

            var isTesting = false;

            var expectedDeliveryBody = new DeliveryBody
            {
                dropoff_name = "Condominio hacienda las palmas",
                dropoff_latitude = 9.94419025278417,
                dropoff_longitude = -84.1809961,
                dropoff_phone_number = "+50689111336",
                dropoff_address = "GAVILANES, POZOS, SANTA ANA, SAN JOSE",
                pickup_name = "RIO ORO",
                pickup_address = "250 metros este del Colegio de Santa Ana",
                pickup_latitude = 9.9335899353027344,
                pickup_longitude = -84.19000244140625,
                pickup_phone_number = "+50647005845",
                manifest_items = new List<ManifestItem>
                {
                    new ManifestItem { name = "AK7", size = "large", quantity = 2 },
                    new ManifestItem { name = "Promo-Pizza Personal", size = "medium", quantity = 2 },
                    new ManifestItem { name = "Promo-Pizza Personal", size = "medium", quantity = 2 },
                    new ManifestItem { name = "Promo-Pizza Personal", size = "medium", quantity = 2 },
                },
                external_store_id = "45",
                dropoff_notes = "200 al este de alimentos la soya",
                manifest_reference = "7842157",
                dropoff_verification = new VerificationRequirement(new SignatureRequirement(true, false, false), false),
                pickup_verification = new VerificationRequirement(new SignatureRequirement(true, false, false), false),
                manifest_total_value = 13765,
                pickup_ready_dt = "2023-05-07T21:29:08.007-06:00"
            };

            if (isTesting)
                expectedDeliveryBody.test_specifications = new TestSpecifications { robo_courier_specification = new RoboCourierSpecification("auto") };

            // Act
            var result = DeliveryService.FromFactura(factura, local, facturaProductos, isTesting, productoEspeciales);

            // Assert
            Assert.AreEqual(expectedDeliveryBody, result);
        }
    }
}
