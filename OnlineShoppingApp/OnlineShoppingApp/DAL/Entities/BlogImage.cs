using OnlineShoppingApp.DAL.Entities.Common;

namespace OnlineShoppingApp.DAL.Entities
{
    public class BlogImage:BaseEntity<int>
    {
        public string ImagePath { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
