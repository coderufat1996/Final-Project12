using OnlineShoppingApp.DAL.Entities.Common;

namespace OnlineShoppingApp.DAL.Entities
{
    public class Size : BaseEntity<int>
    {
        public string Title { get; set; }
        public List<ProductSize>? ProductSizes { get; set; }
    }
}
