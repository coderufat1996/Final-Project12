using OnlineShoppingApp.DAL.Entities.Common;

namespace OnlineShoppingApp.DAL.Entities
{
    public class Blog:BaseEntity<int>
    {
        public string Tittle { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string MainImagePath { get; set; }
        public DateTime CreateTime { get; set; }
        public List<BlogImage> BlogImages { get; set; }
    }
}
