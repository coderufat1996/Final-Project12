using OnlineShoppingApp.DAL.Entities.Common;

namespace OnlineShoppingApp.DAL.Entities
{
    public class ProductCategory : BaseEntity<int>
    {
        public string Title { get; set; }
        public List<Product> Products { get; set; }

    }
}
