namespace PH.UberConnect.Core.Models.Status
{
    public class PickupCompleteStatus : UberStatus
    {
        public PickupCompleteStatus(string deliveryId) : base(deliveryId)
        {
            Area = $"UD Asig #{deliveryId}";
        }

        public override int StatusCode => 3;

        public override string Event => "Motorizado recogió el pedido con éxito";

        public override string Area { get; }
    }
}
