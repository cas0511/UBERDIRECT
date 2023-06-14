using PH.UberConnect.Api.Responses;

namespace PH.UberConnect.Web.Models
{
    public class DeliveryItemViewModel
    {
        public string NumFacturaCall { get; set; }
        public DeliveryResponse Delivery { get; set; }

        public DeliveryItemViewModel(string numFacturaCall, DeliveryResponse deliveryResponse)
        {
            NumFacturaCall = numFacturaCall;
            Delivery = deliveryResponse;
        }
    }
}
