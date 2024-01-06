using OnlineShoppingApp.DAL.Entities.Common;

namespace OnlineShoppingApp.DAL.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MainImagePath { get; set; }
        public decimal Price { get; set; }
        public int Rate { get; set; }
        public int ProductCategoryId { get; set; }
        public List<ProductSize>? ProductSizes { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
