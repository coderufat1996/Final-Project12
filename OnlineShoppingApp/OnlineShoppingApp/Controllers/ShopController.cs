using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;

        public ShopController(OnlineShoppingDbContext onlineShoppingDbContext)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
        }

        public IActionResult Index(int? categoryId , string? search)
        {
            var product = _onlineShoppingDbContext.Products
               .Include(p => p.ProductCategory)
               .AsEnumerable();

            if(categoryId != null)
            {
                product = product.Where(p => p.ProductCategoryId == categoryId);
            }
            else if(search != null)
            {
                product = product.Where(p => p.Name.Contains(search) ||
                p.ProductCategory.Title.Contains(search) ||
                p.Price.ToString().Contains(search) ||
                p.Description.Contains(search));
            }
            var shopViewModel = new ShopViewModel
            {
                ProductCategories = _onlineShoppingDbContext.ProductCategories.ToList(),
                Products = product.ToList()
            };
            return View(shopViewModel);
        }

        [HttpGet("id")]
        public IActionResult ShopItem( int id)
        {
            if (id == null) return NotFound();

            var product = _onlineShoppingDbContext.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductSizes)
                .FirstOrDefault(p => p.Id == id);

            if(product == null) return NotFound();

            var shopItemViewModel = new ShopItemViewModel
            {
                Product = product,
                Products = _onlineShoppingDbContext.Products.Where(p => p.ProductCategoryId == product.ProductCategoryId).ToList(),
                Sizes = _onlineShoppingDbContext.ProductSizes.Where(ps => ps.ProductId==product.Id).Select(ps => new SizeViewModel(ps.Id , ps.Size.Title)).ToList()


            };
            return View(shopItemViewModel);
        }
    }
}
