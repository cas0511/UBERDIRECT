using PH.UberConnect.Core.Interface;
using PH.UberConnect.Core.Models;

namespace PH.UberConnect.Core.Service
{
    public class ItemSizeService
    {
        private IRepositories Repositories { get; set; }

        public ItemSizeService(IRepositories repositories)
        {
            Repositories = repositories;
        }

        public List<ProductoEspecial> Get()
        {
            return Repositories.ItemSizeRepository.Get<ProductoEspecial>();
        }
    }
}
