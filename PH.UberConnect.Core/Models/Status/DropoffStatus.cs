namespace PH.UberConnect.Core.Models.Status
{
    public class DropoffStatus : UberStatus
    {
        public DropoffStatus(string deliveryId) : base(deliveryId)
        {
            Area = $"UD En Ruta #{deliveryId}";
        }

        public override int StatusCode => 8;

        public override string Event => "En camino a entregar el pedido hacia el cliente";

        public override string Area { get; }
    }
}
