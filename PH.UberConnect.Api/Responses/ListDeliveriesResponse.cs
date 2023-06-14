namespace PH.UberConnect.Api.Responses
{
    public class ListDeliveriesResponse
    {
        public int total_count { get; set; }
        public List<DeliveryResponse>? data { get; set; }
    }
}
