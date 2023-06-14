using PH.UberConnect.Core.Interface;
using PH.UberConnect.Core.Models;

namespace PH.UberConnect.Core.Service
{
    public class FacturaProductoService
    {
        private IRepositories _repositories;

        public FacturaProductoService(IRepositories repositories)
        {
            _repositories = repositories;
        }

        public List<FacturaProducto> Get(int numFacturaWeb)
        {
            return _repositories.FacturaProductoRepository.Get(numFacturaWeb);
        }
    }
}
