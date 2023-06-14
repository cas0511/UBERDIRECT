namespace PH.UberConnect.Core.Models
{
    public class Factura
    {
        public int IdFactura { get; set; }
        public string NumFacturaCall { get; set; } = string.Empty;
        public int EstatusUD { get; set; }
        public string? IdRestauranteCliente { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public double MontoTotal { get; set; }
        public DateTime FechaFacturado { get; set; }
        public string? Nombre { get; set; }
    }
}
