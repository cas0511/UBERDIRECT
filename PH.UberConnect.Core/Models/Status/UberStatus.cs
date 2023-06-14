namespace PH.UberConnect.Core.Models.Status
{
    public abstract class UberStatus
    {
        public string DeliveryId { get; set; }
        public abstract int StatusCode { get; }
        public abstract string Event { get; }
        public abstract string Area { get; }

        public UberStatus(string deliveryId)
        {
            DeliveryId = deliveryId;
        }
    }
}
