namespace PH.UberConnect.Webhook.AppSettings
{
    public class SigningKeys
    {
        public string CourierEvent { get; set; }
        public string DeliveryUpdateEvent { get; set; }

        public SigningKeys()
        {

        }

        public SigningKeys(string courierEvent, string deliveryUpdateEvent)
        {
            CourierEvent = courierEvent;
            DeliveryUpdateEvent = deliveryUpdateEvent;
        }
    }
}
