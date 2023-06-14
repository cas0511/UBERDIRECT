using PH.UberConnect.Core.Connection;
using PH.UberConnect.Core.Interface;
using PH.UberConnect.Core.Models;

namespace PH.UberConnect.Core.Repositories
{
    public class ItemSizeRepository : IRepository, IRead
    {
        public string Table => "[dbo].[tbUDProdEsp]";

        public IConnection Connection { get; }

        public ItemSizeRepository(IConnection connection)
        {
            Connection = connection;
        }

        public List<ProductoEspecial> Get<ProductoEspecial>()
        {
            var query = $"SELECT * FROM {IRepository.ServerName}.{Table}";
            return Connection.RetrieveList<ProductoEspecial>(query);
        }
    }
}
