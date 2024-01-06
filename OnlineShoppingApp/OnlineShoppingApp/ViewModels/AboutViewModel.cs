using OnlineShoppingApp.DAL.Entities;

namespace OnlineShoppingApp.ViewModels
{
    public class AboutViewModel
    {
        public About About { get; set; }
        public Banner Banner { get; set; }

        public List<Benefit> Benefits { get; set; }
    }
}
