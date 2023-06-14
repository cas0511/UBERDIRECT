using PH.UberConnect.Core.Connection;
using PH.UberConnect.Core.Interface;

namespace PH.UberConnect.Core.Repositories
{
    public class Repositories : IRepositories
    {
        private readonly IConnection _connection;

        public Repositories(string connectionString)
        {
            _connection = new DapperConnection(connectionString);
        }

        private FacturaRepository _facturaRepository;
        private LocalesRepository _locationsRepository;
        private FacturaProductoRepository _facturaProductoRepository;
        private DeliveryRepository _deliveryRepository;
        private DeliveryHistoryRepository _deliveryHistoryRepository;
        private ItemSizeRepository _itemSizeRepository;

        public FacturaRepository FacturaRepository { get { return _facturaRepository ??= new FacturaRepository(_connection); } }

        public LocalesRepository LocalesRepository { get { return _locationsRepository ??= new LocalesRepository(_connection); } }

        public FacturaProductoRepository FacturaProductoRepository { get { return _facturaProductoRepository ??= new FacturaProductoRepository(_connection); } }

        public DeliveryRepository DeliveryRepository { get { return _deliveryRepository ??= new DeliveryRepository(_connection); } }

        public DeliveryHistoryRepository DeliveryHistoryRepository { get { return _deliveryHistoryRepository ??= new DeliveryHistoryRepository(_connection); } }

        public ItemSizeRepository ItemSizeRepository { get { return _itemSizeRepository ??= new ItemSizeRepository(_connection); } }
    }
}
