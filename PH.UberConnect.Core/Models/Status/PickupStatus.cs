namespace PH.UberConnect.Core.Models.Status
{
    public class PickupStatus : UberStatus
    {
        public PickupStatus(string deliveryId) : base(deliveryId)
        {
            Area = $"UD Asig #{deliveryId}";
        }

        public override int StatusCode => 2;

        public override string Event => "Motorizado asignado y en camino para recoger el pedido";

        public override string Area { get; }
    }
}
