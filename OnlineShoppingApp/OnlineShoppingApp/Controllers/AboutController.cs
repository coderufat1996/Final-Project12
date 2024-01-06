using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class AboutController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;

        public AboutController(OnlineShoppingDbContext onlineShoppingDbContext)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
        }

        public IActionResult Index()
        {
            var aboutViewModel = new AboutViewModel
            {
                Banner = _onlineShoppingDbContext.Banners.FirstOrDefault(b => b.BannerPageType == DAL.Entities.Enums.BannerPageType.About),
                About = _onlineShoppingDbContext.Abouts.FirstOrDefault(),
                Benefits = _onlineShoppingDbContext.Benefits.ToList()
            };
            return View(aboutViewModel);
        }
    }
}
