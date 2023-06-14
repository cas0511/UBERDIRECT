using PH.UberConnect.Api.Responses;
using PH.UberConnect.Core.Connection;
using PH.UberConnect.Core.Interface;
using PH.UberConnect.Core.Models;
using PH.UberConnect.Webhook.Models.DeliveryStatusModels;

namespace PH.UberConnect.Core.Repositories
{
    public class DeliveryRepository : IRepository
    {
        public IConnection Connection { get; }

        public string Table => "[dbo].[tbUDControl]";

        public DeliveryRepository(IConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Inserts a delivery into a table
        /// 
        /// </summary>
        public int Insert(DeliveryResponse deliveryResponse, Factura factura, string jsonFactura)
        {
            var query = $"INSERT INTO {IRepository.ServerName}.{Table} (IdFactura, IdEntregaLarga, IdEntregaCorta, NumFacturaCall, UUID, JsonFactura) "
                + $"VALUES (@IdFactura, @IdEntregaLarga, @IdEntregaCorta, @NumFacturaCall, @UUID, @JsonFactura)";
            var idEntregaCorta = deliveryResponse.uuid![^5..];
            var @params = new
            {
                IdFactura = factura.IdFactura,
                IdEntregaLarga = deliveryResponse.id,
                IdEntregaCorta = idEntregaCorta.ToUpper(),
                NumFacturaCall = factura.NumFacturaCall,
                UUID = deliveryResponse.uuid,
                JsonFactura = jsonFactura,
            };
            return Connection.ExecuteQuery(query, @params);
        }

        /// <summary>
        /// Saves the data of the courier of a delivery
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int UpdateCourier(Data data)
        {
            var query = $"UPDATE {IRepository.ServerName}.{Table} SET NombreMotorizado = @NombreMotorizado, TipoVehiculo = @TipoVehiculo, "
                + $"TelefonoMotorizado = @TelefonoMotorizado WHERE IdFactura = @IdFactura";
            var @params = new
            {
                IdFactura = data.manifest.reference,
                NombreMotorizado = data.courier.name,
                TipoVehiculo = data.courier.vehicle_type,
                TelefonoMotorizado = data.courier.phone_number,
            };
            return Connection.ExecuteQuery(query, @params);
        }

        public int UpdateDateTimes(Data data, int idFactura)
        {
            var query = $"UPDATE {IRepository.ServerName}.{Table} SET PickUpReady = @PickUpReady, PickUpEta = @PickUpEta, PickUpDeadline = @PickUpDeadline, "
                    + $"DropoffReady = @DropoffReady, DropoffEta = @DropoffEta, DropoffDeadline = @DropoffDeadline WHERE IdFactura = @IdFactura";
            var @params = new
            {
                IdFactura = idFactura,
                PickUpReady = data.pickup_ready.AddHours(-6),
                PickUpEta = data.pickup_eta.AddHours(-6),
                PickUpDeadline = data.pickup_deadline.AddHours(-6),
                DropoffReady = data.dropoff_ready.AddHours(-6),
                DropoffEta = data.dropoff_eta.AddHours(-6),
                DropoffDeadline = data.dropoff_deadline.AddHours(-6),
            };
            return Connection.ExecuteQuery(query, @params);
        }

        public List<Factura> GetTodaysFactura()
        {
            var query = $"SELECT NumFacturaCall, IdFactura FROM {IRepository.ServerName}.{Table} " +
                $"WHERE FechaRegistro >= CAST(convert(varchar(10), GETDATE(), 112) AS datetime)";
            return Connection.RetrieveList<Factura>(query);
        }

        public int UpdateCancellationReason(string reason, string personInCharge, string deliveryId)
        {
            var query = $"UPDATE {IRepository.ServerName}.{Table} SET RazonNoEntregable = @RazonNoEntregable, Encargado = @Encargado "
                    + $"WHERE IdEntregaLarga = @IdEntregaLarga";
            var @params = new
            {
                RazonNoEntregable = reason,
                Encargado = personInCharge,
                IdEntregaLarga = deliveryId,
            };
            return Connection.ExecuteQuery(query, @params);
        }
    }
}
