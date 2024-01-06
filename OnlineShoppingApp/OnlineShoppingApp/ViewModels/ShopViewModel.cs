using OnlineShoppingApp.DAL.Entities;

namespace OnlineShoppingApp.ViewModels
{
    public class ShopViewModel
    {
        public List<ProductCategory> ProductCategories { get; set; }
        public List<Product> Products { get; set; }
    }
}
