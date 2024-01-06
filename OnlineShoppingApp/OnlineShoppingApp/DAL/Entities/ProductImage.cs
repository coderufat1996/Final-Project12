using OnlineShoppingApp.DAL.Entities.Common;

namespace OnlineShoppingApp.DAL.Entities
{
    public class ProductImage : BaseEntity<int>
    {
        public string ImagePath { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
