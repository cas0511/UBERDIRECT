namespace PH.UberConnect.Api.Responses.Delivery
{
    public class Dropoff
    {
        public string name { get; set; }
        public string phone_number { get; set; }
        public string address { get; set; }
        public DetailedAddress detailed_address { get; set; }
        public string notes { get; set; }
        public Location location { get; set; }
        public string status { get; set; }
        public DateTime status_timestamp { get; set; }
        public Verification verification { get; set; }
        public VerificationRequirements verification_requirements { get; set; }
    }
}
