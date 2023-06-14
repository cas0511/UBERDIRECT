using System.Text.Json;

namespace PH.UberConnect.Core.Models
{
    public class Tiempos
    {
        public int TiempoPreparacion { get; set; }
        public int TiempoEspera { get; set; }
        public DateTime HoraEspera { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
