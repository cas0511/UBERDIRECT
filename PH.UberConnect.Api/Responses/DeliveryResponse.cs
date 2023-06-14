using PH.UberConnect.Api.Responses.Delivery;
using PH.UberConnect.Webhook.Models.DeliveryStatusModels;

namespace PH.UberConnect.Api.Responses
{
    public class DeliveryResponse
    {
        public bool complete { get; set; }
        public bool courier_imminent { get; set; }
        public bool live_mode { get; set; }
        public DateTime created { get; set; }
        public DateTime dropoff_deadline { get; set; }
        public DateTime dropoff_eta { get; set; }
        public DateTime dropoff_ready { get; set; }
        public DateTime pickup_deadline { get; set; }
        public DateTime pickup_eta { get; set; }
        public DateTime pickup_ready { get; set; }
        public DateTime updated { get; set; }
        public Dropoff? dropoff { get; set; }
        public int fee { get; set; }
        public List<ManifestItem>? manifest_items { get; set; }
        public Manifest? manifest { get; set; }
        public Courier? courier { get; set; }
        public object? fences { get; set; }
        public object? items_acquired { get; set; }
        public object? refunds { get; set; }
        public object? related_deliveries { get; set; }
        public object? state_changes { get; set; }
        public Pickup? pickup { get; set; }
        public PickupPayment? pickup_payment { get; set; }
        public Return? @return { get; set; }
        public string? currency { get; set; }
        public string? deliverable_action { get; set; }
        public string? external_id { get; set; }
        public string? id { get; set; }
        public string? kind { get; set; }
        public string? pickup_action { get; set; }
        public string? quote_id { get; set; }
        public string? status { get; set; }
        public string? tracking_url { get; set; }
        public string? undeliverable_action { get; set; }
        public string? undeliverable_reason { get; set; }
        public string? uuid { get; set; }
    }
}
