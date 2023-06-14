using PH.UberConnect.Api.Responses.Delivery;

namespace PH.UberConnect.Webhook.Models.DeliveryStatusModels
{
    public class Courier
    {
        public string img_href { get; set; }
        public Location location { get; set; }
        public string location_description { get; set; }
        public string name { get; set; }
        public string phone_number { get; set; }
        public string rating { get; set; }
        public string vehicle_color { get; set; }
        public string vehicle_make { get; set; }
        public string vehicle_model { get; set; }
        public string vehicle_type { get; set; }
    }
}
