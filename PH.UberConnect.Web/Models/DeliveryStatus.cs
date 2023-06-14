using PH.UberConnect.Webhook.Models.DeliveryStatusModels;

namespace PH.UberConnect.Web.Models
{
    public class DeliveryStatus
    {
        public string account_id { get; set; }
        public string batch_id { get; set; }
        public DateTime created { get; set; }
        public string customer_id { get; set; }
        public Data data { get; set; }
        public string delivery_id { get; set; }
        public string developer_id { get; set; }
        public string id { get; set; }
        public string kind { get; set; }
        public bool live_mode { get; set; }
        public string route_id { get; set; }
        public string status { get; set; }
    }


}
