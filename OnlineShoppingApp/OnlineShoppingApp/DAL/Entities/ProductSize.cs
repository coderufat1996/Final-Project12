using OnlineShoppingApp.DAL.Entities.Common;

namespace OnlineShoppingApp.DAL.Entities
{
    public class ProductSize : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int SizeId { get; set; }
        public Size? Size { get; set; }
    }
}
