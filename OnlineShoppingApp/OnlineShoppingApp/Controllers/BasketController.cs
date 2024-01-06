using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class BasketController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;

        public BasketController(OnlineShoppingDbContext onlineShoppingDbContext)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddBasket(int? id , int? sizeId)
        {
            if (id == null) return NotFound();
            var existProduct = _onlineShoppingDbContext.Products
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == id);
            if (existProduct is null) return NotFound();

            List<BasketViewModel> list = CheckBasket();

            CheckBasketItemCount(list, existProduct , sizeId);

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(list), new CookieOptions { MaxAge = TimeSpan.FromMinutes(30) });
            //return RedirectToAction("index", "home");
            return Redirect(Request.Headers["Referer"].ToString());


        }

        public IActionResult ShowBasket()
        {
            string basket = Request.Cookies["basket"];
            List<BasketViewModel> products = new();
            if (basket != null)
            {
                products = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
                products = UpdateBasket(products);
            }
            return View(products);
        }

        private List<BasketViewModel> UpdateBasket(List<BasketViewModel> products)
        {
            foreach (var basketProduct in products)
            {
                var existProduct = _onlineShoppingDbContext.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefault(p => p.Id == basketProduct.Id);
                basketProduct.Name = existProduct.Name;
                basketProduct.Price = existProduct.Price;
                basketProduct.ImagePath = existProduct.MainImagePath;
                if(basketProduct.BasketCount > 1)
                {
                    basketProduct.BasketPrice = existProduct.Price * basketProduct.BasketCount;
                }
                else
                {
                    basketProduct.BasketPrice = existProduct.Price;
                }
            }
            return products;
        }

        private List<BasketViewModel> CheckBasket()
        {
            List<BasketViewModel> list;
            string basket = Request.Cookies["basket"];
            if (basket == null)
            {
                list = new();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            }
            return list;
        }

        private void CheckBasketItemCount(List<BasketViewModel> list, Product product , int? sizeId)
        {
            var existProductInBasket = list.FirstOrDefault(p => p.Id == product.Id);
            if (existProductInBasket is null)
            {
                BasketViewModel basketVM = new();

                basketVM.Id = product.Id;
                basketVM.BasketCount = 1;
                basketVM.Price = product.Price;
                basketVM.ImagePath = product.MainImagePath;
                basketVM.SizeId = sizeId != null ? sizeId : default;
                basketVM.CategoryId = product.ProductCategoryId;
                basketVM.BasketPrice = product.Price;
                list.Add(basketVM);

            }
            else
            {
                existProductInBasket.BasketCount++;
                existProductInBasket.BasketPrice = existProductInBasket.Price * existProductInBasket.BasketCount;
            }
        }

        public IActionResult Remove(int? id)
        {
            string basket = Request.Cookies["basket"];
            var products = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            var basketItem = products.FirstOrDefault(p => p.Id == id);
            if (basketItem != null)
            {
                products.Remove(basketItem);
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products),
                new CookieOptions { MaxAge = TimeSpan.FromMinutes(15) });
            return RedirectToAction("ShowBasket");

        }

        public IActionResult Increase(int? id)
        {
            string basket = Request.Cookies["basket"];
            var products = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            var increaseProduct = products.FirstOrDefault(p => p.Id == id);

            if (increaseProduct != null)
            {
                if (increaseProduct.BasketCount < 10)
                {
                    increaseProduct.BasketCount++;
                    Response.Cookies.Append("basket", JsonConvert.SerializeObject(products),
                    new CookieOptions { MaxAge = TimeSpan.FromMinutes(15) });
                }
                else
                {
                    return RedirectToAction("ShowBasket");
                }
            }

            return RedirectToAction("ShowBasket");
        }

        public IActionResult Decrease(int? id)
        {

            string basket = Request.Cookies["basket"];
            var products = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            var decreaseProduct = products.FirstOrDefault(p => p.Id == id);

            if (decreaseProduct != null)
            {
                if (decreaseProduct.BasketCount > 1)
                {
                    decreaseProduct.BasketCount--;
                }
                else
                {
                    products.Remove(decreaseProduct);
                }
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products),
                new CookieOptions { MaxAge = TimeSpan.FromMinutes(15) });
            return RedirectToAction("ShowBasket");
        }


    }
}
