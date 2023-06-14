namespace PH.UberConnect.Api.Responses.Delivery
{
    public class Return
    {
        public string name { get; set; }
        public string phone_number { get; set; }
        public string address { get; set; }
        public DetailedAddress detailed_address { get; set; }
        public string notes { get; set; }
        public Location location { get; set; }
        public string external_store_id { get; set; }
    }
}
