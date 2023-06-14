namespace PH.UberConnect.Core.Models.Status
{
    public class ReturnedStatus : UberStatus
    {
        public ReturnedStatus(string deliveryId) : base(deliveryId)
        {
            Area = $"UD Devo #{deliveryId}";
        }

        public override int StatusCode => 7;

        public override string Event => "La entrega fue cancelada, y se crea una nueva entrega para devolver los articulos al remitente";

        public override string Area { get; }
    }
}
