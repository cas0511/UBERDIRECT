using PH.UberConnect.Api.Responses;
using PH.UberConnect.Core.Enums;
using PH.UberConnect.Core.Models;

namespace PH.UberConnect.Web.Models
{
    public class ListDeliveriesViewModel
    {
        public List<DeliveryItemViewModel> Deliveries { get; set; }
        public Local Store { get; set; }
        public bool HasError { get; set; } = false;
        public string? ErrorMessage { get; set; }
        public DeliveryFilter DeliveryFilter { get; set; }

        public ListDeliveriesViewModel()
        {

        }

        public ListDeliveriesViewModel(List<DeliveryItemViewModel> deliveries, Local store, DeliveryFilter deliveryFilter)
        {
            Deliveries = deliveries;
            Store = store;
            DeliveryFilter = deliveryFilter;
        }

        public ListDeliveriesViewModel(List<DeliveryItemViewModel> deliveries, Local store)
        {
            Deliveries = deliveries;
            Store = store;
        }

        public string GetStatusHtmlElement(string status)
        {
            string? className;
            string? statusName;

            switch (status)
            {
                case "pending":
                    statusName = "Buscando motorizado";
                    className = "bg-warning";
                    break;
                case "pickup":
                    statusName = "Motorizado en camino";
                    className = "badge bg-info text-dark";
                    break;
                case "pickup_complete":
                    statusName = "Recogido";
                    className = "bg-primary";
                    break;
                case "dropoff":
                    statusName = "En camino a ser entregado al cliente";
                    className = "badge bg-info text-dark";
                    break;
                case "delivered":
                    statusName = "Entregado";
                    className = "bg-success";
                    break;
                case "canceled":
                    statusName = "Cancelado";
                    className = "bg-light text-dark";
                    break;
                case "returned":
                    statusName = "Devuelto";
                    className = "badge bg-dark";
                    break;
                default:
                    statusName = "¡Sin estado!";
                    className = "bg-warning";
                    break;
            }

            return $"<span class=\"badge {className}\">{statusName}</span>";
        }

        /// <summary>
        /// Filters the list of deliveries of a store, it depends on the filter that it received
        /// </summary>
        public void ApplyFilter()
        {
            var cancelOnlyThoseStatus = new List<string> { "pending", "dropoff", "pickup_complete", "pickup" };

            switch (DeliveryFilter)
            {
                case DeliveryFilter.OnGoing:
                    Deliveries = Deliveries.Where(deliveryItem => cancelOnlyThoseStatus.Contains(deliveryItem.Delivery.status)).ToList();
                    break;
                case DeliveryFilter.Delivered:
                    Deliveries = Deliveries.Where(deliveryItem => deliveryItem.Delivery.status == "delivered").ToList();
                    break;
                case DeliveryFilter.Canceled:
                    Deliveries = Deliveries.Where(deliveryItem => deliveryItem.Delivery.status == "canceled" || deliveryItem.Delivery.status == "returned").ToList();
                    break;
                default:
                    break;
            };
        }
    }
}
