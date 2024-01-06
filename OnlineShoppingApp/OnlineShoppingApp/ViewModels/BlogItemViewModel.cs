using OnlineShoppingApp.DAL.Entities;

namespace OnlineShoppingApp.ViewModels
{
    public class BlogItemViewModel
    {
        public Blog Blog { get; set; }
        public List<Blog> RecentBlogs { get; set; }
    }
}
