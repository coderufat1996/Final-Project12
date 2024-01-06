using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;

        public HomeController(OnlineShoppingDbContext onlineShoppingDbContext)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
        }

        public IActionResult Index()
        {
            var previousUrl = Request.Headers["Referer"].ToString();

            var homeViewModel = new HomeViewModel
            {
                Benefits = _onlineShoppingDbContext.Benefits.Take(6).ToList(),
                Deals = _onlineShoppingDbContext.Deals.Take(4).ToList(),
                Products = _onlineShoppingDbContext.Products.Include(p => p.ProductCategory).Take(8).ToList(),
                NewProducts = _onlineShoppingDbContext.Products.OrderBy(p => p.CreateTime).Take(8).ToList()
            };   

            return View(homeViewModel);
        }
    }
}
