namespace PH.UberConnect.Core.Models.Status
{
    public class DeliveredStatus : UberStatus
    {
        public DeliveredStatus(string deliveryId) : base(deliveryId)
        {
            Area = $"UD Ent #{deliveryId}";
        }

        public override int StatusCode => 5;

        public override string Event => "Pedido entregado con éxito al cliente";

        public override string Area { get; }
    }
}
