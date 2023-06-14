using PH.UberConnect.Core.Interface;

namespace PH.UberConnect.Core.Service
{
    public class Services : IServices
    {
        private IRepositories _repositories;

        private DeliveryService _deliveryService;
        private LocalesService _locationsService;
        private FacturaProductoService _facturaProductoService;
        private FacturaService _facturaService;
        private DeliveryHistoryService _deliveryHistoryService;
        private ItemSizeService _itemSizeService;

        public Services(string connectionString)
        {
            _repositories = new Repositories.Repositories(connectionString);
        }

        public DeliveryService DeliveryService { get { return _deliveryService ??= new DeliveryService(_repositories); } }

        public LocalesService LocalesService { get { return _locationsService ??= new LocalesService(_repositories); } }

        public FacturaProductoService FacturaProductoService { get { return _facturaProductoService ??= new FacturaProductoService(_repositories); } }

        public FacturaService FacturaService { get { return _facturaService ??= new FacturaService(_repositories); } }

        public DeliveryHistoryService DeliveryHistoryService { get { return _deliveryHistoryService ??= new DeliveryHistoryService(_repositories); } }

        public ItemSizeService ItemSizeService { get { return _itemSizeService ??= new ItemSizeService(_repositories); } }
    }
}
