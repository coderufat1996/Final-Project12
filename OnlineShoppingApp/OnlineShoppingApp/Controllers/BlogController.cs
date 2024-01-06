using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;

        public BlogController(OnlineShoppingDbContext onlineShoppingDbContext)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var banner = _onlineShoppingDbContext.Banners.FirstOrDefault(b => b.BannerPageType == DAL.Entities.Enums.BannerPageType.Blog);
            var blogViewModel = new BlogViewModel
            {
                Blogs = _onlineShoppingDbContext.Blogs.ToList(),
                Banner = new BannerViewModel(banner.Tittle, banner.Description, banner.ImagePath)

            };
            return View(blogViewModel);
        }

        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            var blog = _onlineShoppingDbContext.Blogs.Include(b => b.BlogImages).FirstOrDefault(b => b.Id == id);
            if (blog == null) return NotFound();

            var blogItemViewModel = new BlogItemViewModel
            {
                Blog = blog,
                RecentBlogs = _onlineShoppingDbContext.Blogs.Take(4).OrderBy(b => b.CreateTime).ToList()
            };

            return View(blogItemViewModel);
        }
    }
}
