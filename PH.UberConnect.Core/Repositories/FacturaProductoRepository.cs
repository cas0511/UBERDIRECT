using PH.UberConnect.Core.Connection;
using PH.UberConnect.Core.Interface;
using PH.UberConnect.Core.Models;

namespace PH.UberConnect.Core.Repositories
{
    public class FacturaProductoRepository : IRepository, IReadOne<FacturaProducto>
    {
        public IConnection Connection { get; }

        public string Table => "dbo.fnFacturaSendDetalleLvl3";

        public FacturaProductoRepository(IConnection connection)
        {
            Connection = connection;
        }

        public List<FacturaProducto> Get(int numFacturaProducto)
        {
            var query = $"SELECT * FROM OPENQUERY([PHCRSRVMCARIIS0], 'select * from [ClickEat].{Table}({numFacturaProducto})')";
            return Connection.RetrieveList<FacturaProducto>(query);
        }
    }
}
