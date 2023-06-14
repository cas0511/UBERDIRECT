using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PH.UberConnect.Core.Models
{
    public class Local
    {
        public string? CodRest { get; set; }
        public string? Descripcion { get; set; }
        public string? db_origen { get; set; }
        public string? NombBase { get; set; }
        public string? UserId { get; set; }
        public string? PasWd { get; set; }
        public bool activo { get; set; }
        public string? IPServer { get; set; }
        public string? Enlace { get; set; }
        public string? PH_IP { get; set; }
        public string? Vinculo { get; set; }
        public string? POS { get; set; }
        public int IdRestaurant { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public int IntentosUD { get; set; } = 0;
        public int TiempoEntrega { get; set; }
        public int MinutosDT { get; set; }
        public bool ActivoUD { get; set; }
        public bool ActulTiempo { get; set; }
        public string? DirecNota { get; set; }

        override
        public string ToString() => JsonSerializer.Serialize(this);

        public string GetConnString()
        {
            if (string.IsNullOrEmpty(db_origen)
                || string.IsNullOrEmpty(NombBase)
                || string.IsNullOrEmpty(PasWd)
                || string.IsNullOrEmpty(UserId))
                throw new Exception($"Conexion a la BD de la tienda {this.Descripcion} imcompleta.");

            return "Server=" + db_origen + ";Database=" + NombBase + ";User Id =" + UserId + ";Password=" + PasWd;
        }
    }
}
