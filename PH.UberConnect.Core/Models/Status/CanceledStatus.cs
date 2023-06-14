namespace PH.UberConnect.Core.Models.Status
{
    public class CanceledStatus : UberStatus
    {
        public CanceledStatus(string deliveryId) : base(deliveryId)
        {
            Area = $"UD Canc #{deliveryId}";
        }

        public override int StatusCode => 6;

        public override string Event => "Pedido fue cancelada";

        public override string Area { get; }
    }
}
