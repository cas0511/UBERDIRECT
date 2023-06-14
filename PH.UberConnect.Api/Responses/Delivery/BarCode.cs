namespace PH.UberConnect.Api.Responses.Delivery
{
    public class Barcode
    {
        public string type { get; set; }
        public string value { get; set; }
        public ScanResult scan_result { get; set; }
    }
}
