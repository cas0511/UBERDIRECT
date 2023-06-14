
namespace PH.UberConnect.Core.Models.Status
{
    public class PendingStatus : UberStatus
    {
        public override int StatusCode { get; }

        public override string Event { get; }

        public override string Area { get; }

        public PendingStatus(string deliveryId) : base(deliveryId)
        {
            StatusCode = 1;
            Event = "Pedido creado con éxito, pero aún no tiene motorizado asignado";
            Area = $"UD S. #{deliveryId}";
        }
    }
}
