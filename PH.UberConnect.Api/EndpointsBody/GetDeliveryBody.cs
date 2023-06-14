namespace PH.UberConnect.Api.EndpointsBody
{
    public class GetDeliveryBody : IEndpointBody
    {
        public string DeliveryId { get; set; }

        public GetDeliveryBody(string deliveryId)
        {
            DeliveryId = deliveryId;
        }
    }
}
