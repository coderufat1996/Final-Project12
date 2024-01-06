using OnlineShoppingApp.DAL.Entities;

namespace OnlineShoppingApp.ViewModels
{
    public class HomeViewModel
    {
        public List<Benefit> Benefits { get; set; }
        public List<Deal> Deals { get; set; }

        public List<Product> Products { get; set; }
        public List<Product> NewProducts { get; set; }
    }
}
