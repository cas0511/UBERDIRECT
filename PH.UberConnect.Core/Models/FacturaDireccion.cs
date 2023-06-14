namespace PH.UberConnect.Core.Models
{
    public class FacturaDireccion : Factura
    {
        public string? NombreProvincia { get; set; }
        public string? NombreCanton { get; set; }
        public string? NombreDistrito { get; set; }
        public string? NombreBarrio { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string? NombreDireccion { get; set; }
        public string? Direccion { get; set; }
        public string? PuntoReferencia { get; set; }
    }
}
