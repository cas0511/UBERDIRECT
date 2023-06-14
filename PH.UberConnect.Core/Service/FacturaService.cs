using PH.UberConnect.Core.Interface;
using PH.UberConnect.Core.Models;

namespace PH.UberConnect.Core.Service
{
    public class FacturaService
    {
        private IRepositories _repositories { get; set; }

        public FacturaService(IRepositories repositories)
        {
            _repositories = repositories;
        }

        public List<FacturaDireccion> Get(List<string> localIdList)
        {
            return _repositories.FacturaRepository.Get<FacturaDireccion>()
                                                  .Where(fac => localIdList.Contains(fac.IdRestauranteCliente))
                                                  .ToList();
        }

        public int UpdateState(int idFactura, int estatusUD)
        {
            return _repositories.FacturaRepository.UpdateState(idFactura, estatusUD);
        }

        public FacturaDireccion GetById(string idFactura)
        {
            return _repositories.FacturaRepository.Get(idFactura);
        }

        /// <summary>
        /// Gets the facturas group by locales
        /// 
        /// Creates a dictionary where each entry references a store and its value is the list of receipts
        /// </summary>
        public Dictionary<Local, List<FacturaDireccion>> GetFacturasGroupByLocales(List<FacturaDireccion> facturas, List<Local> locales, List<DeliveryHistory> histories)
        {
            var facturasByLocal = new Dictionary<Local, List<FacturaDireccion>>();

            foreach (var factura in facturas)
            {
                var currentLocal = locales.First(local => local.CodRest == factura.IdRestauranteCliente);

                if (!facturasByLocal.ContainsKey(currentLocal))
                {
                    facturasByLocal.Add(currentLocal, new List<FacturaDireccion>());
                }

                var attemptsCount = histories.Where(h => h.IdFactura == factura.IdFactura).ToList().Count;

                if (attemptsCount < currentLocal.IntentosUD)
                {
                    facturasByLocal[currentLocal].Add(factura);
                }
            }

            return facturasByLocal;
        }
    }
}
