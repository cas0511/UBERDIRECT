using PH.UberConnect.Core.Connection;
using PH.UberConnect.Core.Interface;
using PH.UberConnect.Core.Models;

namespace PH.UberConnect.Core.Repositories
{
    public class FacturaRepository : IRepository, IRead
    {
        public IConnection Connection { set; get; }

        public string Table => "[dbo].[tbFactura]";

        public FacturaRepository(IConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Gets a today's list of facturas 
        /// </summary>
        /// <typeparam name="FacturaDireccion"></typeparam>
        /// <returns></returns>
        public List<FacturaDireccion> Get<FacturaDireccion>()
        {
            var query = "SELECT F.EnviadoApi, F.IdFactura, F.NumFacturaCall, F.EstatusUD, F.IdRestauranteCliente, F.Telefono, F.MontoTotal, "
                + "F.FechaFacturado, F.Nombre, FD.* "
                + $"FROM {IRepository.ServerName}.{Table} F "
                + $"INNER JOIN {IRepository.ServerName}.[dbo].[tbFacturaDireccion] FD ON F.IdFactura = FD.IdFactura "
                + "WHERE "
                + "F.FechaFacturado >= CAST(convert(varchar(10), GETDATE(), 112) AS datetime) AND "
                + "F.Numerotarjeta <> 'XXXX' AND F.Tipoentrega = 'E' AND EstatusUD = 0 "
                + "AND F.NumFacturaCall <> 0 "
                + "ORDER BY F.Fechafacturado DESC";
            var facturas = Connection.RetrieveList<FacturaDireccion>(query);
            return facturas;
        }

        /// <summary>
        /// Updates the estatusUD of a factura by [idFactura] 
        /// </summary>
        public int UpdateState(int idFactura, int estatusUD)
        {
            var query = $"UPDATE {IRepository.ServerName}.{Table} SET EstatusUD = @estatusUD WHERE IdFactura = @idFactura";
            var @params = new { estatusUD, idFactura };
            return Connection.ExecuteQuery(query, @params);
        }

        /// <summary>
        /// Gets a one factura by idFactura
        /// </summary>
        public FacturaDireccion Get(string idFactura)
        {
            var query = "SELECT F.EnviadoApi, F.IdFactura, F.NumFacturaCall, F.EstatusUD, F.IdRestauranteCliente, F.Telefono, FD.* "
                + $"FROM {IRepository.ServerName}.{Table} F "
                + $"INNER JOIN {IRepository.ServerName}.[dbo].[tbFacturaDireccion] FD ON F.IdFactura = FD.IdFactura "
                + "WHERE F.IdFactura = @idFactura";
            var @params = new { idFactura };
            return Connection.RetrieveList<FacturaDireccion>(query, @params).First();
        }
    }
}
