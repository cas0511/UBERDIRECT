using PH.UberConnect.Core.Service;

namespace PH.UberConnect.Core.Interface
{
    public interface IServices
    {
        public DeliveryService DeliveryService { get; }
        public LocalesService LocalesService { get; }
        public FacturaProductoService FacturaProductoService { get; }
        public FacturaService FacturaService { get; }
        public DeliveryHistoryService DeliveryHistoryService { get; }
        public ItemSizeService ItemSizeService { get; }
    }
}
