using PH.UberConnect.Core.Interface;
using PH.UberConnect.Core.Models;
using PH.UberConnect.Core.Models.Status;

namespace PH.UberConnect.Core.Service
{
    public class DeliveryHistoryService
    {
        public IRepositories Repositories{ get; set; }

        public DeliveryHistoryService(IRepositories repositories)
        {
            Repositories = repositories;
        }

        public int Insert(Factura factura, UberStatus uberStatus, string jsonResponse)
        {
            var history = new DeliveryHistory(factura.IdFactura, factura.NumFacturaCall, factura.IdRestauranteCliente, uberStatus.Event, uberStatus.StatusCode, jsonResponse);
            return Repositories.DeliveryHistoryRepository.Insert(history);
        }

        /// <summary>
        /// Gets only the receipts's errors histories
        /// 
        /// This method is used for get the count of attempts of the receipts.
        /// The attempt count is major than the attempts allowed by store, that receipt will not send to Uber
        /// </summary>
        public List<DeliveryHistory> Get()
        {
            return Repositories.DeliveryHistoryRepository.Get();
        }

        public UberStatus GetEventStatus(string uberStatus, string deliveryId)
        {
            return uberStatus switch
            {
                "pending" => new PendingStatus(deliveryId),
                "pickup" => new PickupStatus(deliveryId),
                "pickup_complete" => new PickupCompleteStatus(deliveryId),
                "dropoff" => new DropoffStatus(deliveryId),
                "delivered" => new DeliveredStatus(deliveryId),
                "canceled" => new CanceledStatus(deliveryId),
                "returned" => new ReturnedStatus(deliveryId),
                _ => new PendingStatus(deliveryId),
            };
        }
    }
}
