namespace PH.UberConnect.Core.Models.Status
{
    public class ErrorStatus : UberStatus
    {
        public override int StatusCode { get; }

        public override string Event { get; }

        public override string Area { get; }

        public ErrorStatus(string @event, int statusCode, string deliveryId) : base(deliveryId)
        {
            Event = @event;
            StatusCode = statusCode;
            Area = "UD Error";
        }
    }
}
