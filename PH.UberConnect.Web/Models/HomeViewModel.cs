using PH.UberConnect.Core.Models;

namespace PH.UberConnect.Web.Models
{
    public class HomeViewModel
    {
        public List<Local> Stores { get; set; }
        public string? StoreIdSelected { get; set; }
        public string? StoreIdInputValue { get; set; }

        public HomeViewModel()
        {

        }

        public HomeViewModel(List<Local> stores)
        {
            Stores = stores;
        }
    }
}
