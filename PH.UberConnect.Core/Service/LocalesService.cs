using PH.UberConnect.Core.Connection;
using PH.UberConnect.Core.Interface;
using PH.UberConnect.Core.Models;
using PH.UberConnect.Webhook.Models.DeliveryStatusModels;

namespace PH.UberConnect.Core.Service
{
    public class LocalesService
    {
        private IRepositories _repositories;

        public LocalesService(IRepositories repositories)
        {
            _repositories = repositories;
        }

        /// <summary>
        /// Gets all stores
        /// </summary>
        public List<Local> Get()
        {
            return _repositories.LocalesRepository.Get<Local>()
                                                  .Where(l => l.ActivoUD == true)
                                                  .ToList();
        }

        /// <summary>
        /// Gets a store by id
        /// </summary>
        public Local Get(string? id)
        {
            return _repositories.LocalesRepository.Get(id);
        }

        /// <summary>
        /// Updates the receipt's area of a store.
        /// 
        /// Needs the database connection of a store.
        /// </summary>
        public int Update(string numFacturaCallCenter, Local local, string nomArea = "Sin Area 1")
        {
            IConnection localConnection = new DapperConnection(local.GetConnString());
            return _repositories.LocalesRepository.Update(numFacturaCallCenter, localConnection, nomArea);
        }

        /// <summary>
        /// Update receipts area of a store
        /// </summary>
        /// <param name="deliveryIdlist"></param>
        /// <param name="local"></param>
        /// <param name="nomArea"></param>
        /// <returns></returns>
        public int UpdateIn(Dictionary<string, Factura> facturasSent, Local local)
        {
            IConnection localConnection = new DapperConnection(local.GetConnString());
            return _repositories.LocalesRepository.UpdateIn(facturasSent, localConnection);
        }

        /// <summary>
        /// Updates some data of a receipt when it is going to be closed
        /// </summary>
        /// <param name="numFacturaCallCenter"></param>
        /// <param name="local"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int CloseOrder(string numFacturaCallCenter, Local local, DateTime dropoffReady, Tiempos tiempos)
        {
            IConnection localConnection = new DapperConnection(local.GetConnString());

            return _repositories.LocalesRepository.CloseOrder(numFacturaCallCenter, dropoffReady, localConnection, tiempos);
        }

        /// <summary>
        /// Gets the TiempoPreparacion, TiempoEspera and HoraEspera of a receipt
        /// </summary>
        /// <param name="numFacturaCallCenter"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public Tiempos GetTiempos(string numFacturaCallCenter, Local local)
        {
            IConnection localConnection = new DapperConnection(local.GetConnString());
            return _repositories.LocalesRepository.GetTiempos(numFacturaCallCenter, localConnection);
        }

        /// <summary>
        /// Updates the field MinutosDT of a store
        /// </summary>
        /// <param name="codRest"></param>
        /// <param name="minutosDT"></param>
        /// <returns></returns>
        public int UdpateMinutosDT(string codRest, int minutosDT)
        {
            return _repositories.LocalesRepository.UdpateMinutosDT(codRest, minutosDT);
        }
    }
}
