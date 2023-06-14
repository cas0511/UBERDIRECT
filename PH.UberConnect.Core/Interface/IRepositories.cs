using PH.UberConnect.Core.Repositories;

namespace PH.UberConnect.Core.Interface
{
    public interface IRepositories
    {
        public FacturaRepository FacturaRepository{ get; }
        public LocalesRepository LocalesRepository { get; }
        public FacturaProductoRepository FacturaProductoRepository { get; }
        public DeliveryRepository DeliveryRepository { get; }
        public DeliveryHistoryRepository DeliveryHistoryRepository { get; }
        public ItemSizeRepository ItemSizeRepository { get; }
    }
}
