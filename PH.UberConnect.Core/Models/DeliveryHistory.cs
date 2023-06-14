namespace PH.UberConnect.Core.Models
{
    public class DeliveryHistory
    {
        public int IdFactura { get; set; }
        public string NumFacturaCall { get; set; }
        public string CodRest { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Evento { get; set; }
        public int Estado { get; set; }
        public string JsonResponse { get; set; }

        public DeliveryHistory(int idFactura, string numFacturaCall, string codRest, string? evento, int estado, string jsonResponse)
        {
            IdFactura = idFactura;
            NumFacturaCall = numFacturaCall;
            CodRest = codRest;
            Evento = evento;
            Estado = estado;
            JsonResponse = jsonResponse;
        }

        public DeliveryHistory()
        {

        }
    }
}
