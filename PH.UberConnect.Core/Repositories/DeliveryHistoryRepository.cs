using PH.UberConnect.Core.Connection;
using PH.UberConnect.Core.Interface;
using PH.UberConnect.Core.Models;

namespace PH.UberConnect.Core.Repositories
{
    public class DeliveryHistoryRepository : IRepository
    {
        public IConnection Connection { get; }

        public string Table => "[dbo].[TbUDHistMov]";

        public DeliveryHistoryRepository(IConnection connection)
        {
            Connection = connection;
        }

        public int Insert(DeliveryHistory deliveryHistory)
        {
            var query = $"INSERT INTO {IRepository.ServerName}.{Table} (IdFactura, NumFacturaCall, CodRest, Evento, Estado, JsonResponse) "
                + $"VALUES (@IdFactura, @NumFacturaCall, @CodRest, @Evento, @Estado, @JsonResponse)";
            return Connection.ExecuteQuery(query, deliveryHistory);
        }

        public List<DeliveryHistory> Get()
        {
            var query = $"SELECT * FROM {IRepository.ServerName}.{Table} "
                + "WHERE Fecha >= CAST(CONVERT(varchar(10), GETDATE(), 112) AS datetime) "
                + "AND Estado in (-1, -2)";
            
            return Connection.RetrieveList<DeliveryHistory>(query);
        }
    }
}
