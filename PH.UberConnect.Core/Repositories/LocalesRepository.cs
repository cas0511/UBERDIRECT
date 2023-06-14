using PH.UberConnect.Core.Connection;
using PH.UberConnect.Core.Interface;
using PH.UberConnect.Core.Models;
using PH.UberConnect.Core.Models.Status;
using System.Text;

namespace PH.UberConnect.Core.Repositories
{
    public class LocalesRepository : IRepository, IRead
    {
        public IConnection Connection { get; }

        public string Table => "[dbo].Locales";

        public LocalesRepository(IConnection connection)
        {
            Connection = connection;
        }

        public List<Local> Get<Local>()
        {
            var query = $"SELECT * FROM {IRepository.ServerName}.{Table}";
            return Connection.RetrieveList<Local>(query);
        }

        public Local Get(string id)
        {
            var query = $"SELECT * FROM {IRepository.ServerName}.{Table} WHERE CodRest = @id";
            var @params = new { id };
            return Connection.RetrieveList<Local>(query, @params).First();
        }

        public int Update(string numFacturaCallCenter, IConnection localConnection, string nomArea)
        {
            var query = "UPDATE [dbo].[Tiempos] SET NomArea = @nomArea WHERE NumfacturaCallCenter = @numFacturaCallCenter";
            var _params = new { numFacturaCallCenter, nomArea };
            return localConnection.ExecuteQuery(query, _params);
        }

        public int UpdateIn(Dictionary<string, Factura> facturasSent, IConnection localConnection)
        {
            var mainQuery = new StringBuilder();

            foreach (KeyValuePair<string, Factura> factura in facturasSent)
            {
                var uberStatus = new PendingStatus(factura.Key[^5..].ToUpper());
                var query = $"UPDATE [dbo].[Tiempos] SET NomArea = '{uberStatus.Area}' WHERE NumfacturaCallCenter = '{factura.Value.NumFacturaCall}';";
                mainQuery.Append(query);

            }

            return localConnection.ExecuteQuery(mainQuery.ToString());
        }

        public Tiempos GetTiempos(string numFacturaCallCenter, IConnection localConnection)
        {
            var selectQuery = "SELECT TiempoPreparacion, TiempoEspera, HoraEspera FROM [dbo].[Tiempos] WHERE NumFacturaCallCenter = @numFacturaCallCenter";
            var selectParams = new { numFacturaCallCenter };
            return localConnection.RetrieveList<Tiempos>(selectQuery, selectParams).First();
        }

        public int CloseOrder(string numFacturaCallCenter, DateTime dropoffReady, IConnection localConnection, Tiempos tiempo)
        {
            var tiempoTotal = GetTiempoTotal(dropoffReady, tiempo.HoraEspera, tiempo.TiempoPreparacion, tiempo.TiempoEspera);
            var horaRegreso = dropoffReady - tiempo.HoraEspera;

            var query = "UPDATE [dbo].[Tiempos] SET Tiemporegreso = @tiempoRegreso, TiempoTotal = @tiempoTotal, FinRegreso = 1 WHERE NumfacturaCallCenter = @numFacturaCallCenter";
            var @params = new { numFacturaCallCenter, tiempoTotal, tiempoRegreso = horaRegreso.TotalMinutes };
            return localConnection.ExecuteQuery(query, @params);
        }

        public int GetTiempoTotal(DateTime dropoffReadyHour, DateTime horaEspera, int tiempoPreparacion, int tiempoEspera)
        {
            var horaRegreso = dropoffReadyHour - horaEspera;
            var tiempoTotal = tiempoPreparacion + tiempoEspera + horaRegreso.TotalMinutes;
            return Convert.ToInt32(tiempoTotal);
        }

        public int UdpateMinutosDT(string codRest, int minutosDT)
        {
            var query = $"UPDATE {IRepository.ServerName}.{Table} SET MinutosDT = @minutosDT WHERE CodRest = @codRest";
            var @params = new { minutosDT, codRest };
            return Connection.ExecuteQuery(query, @params);
        }
    }
}
