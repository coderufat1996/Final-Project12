using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Areas.Admin.ViewModels.Category;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;

        public CategoryController(OnlineShoppingDbContext onlineShoppingDbContext)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View(_onlineShoppingDbContext.ProductCategories.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductCategory category)
        {
            var newcategory = new ProductCategory
            {
                Title = category.Title
            };

            _onlineShoppingDbContext.ProductCategories.Add(newcategory);

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var findedcategory = _onlineShoppingDbContext.ProductCategories.FirstOrDefault(s => s.Id == id);

            if (findedcategory == null) return NotFound();

            var categoryUpdateViewModel = new UpdateViewModel
            {
                Id = findedcategory.Id,
                Title = findedcategory.Title
            };

            return View(categoryUpdateViewModel);
        }

        [HttpPost]
        public IActionResult Update(UpdateViewModel updateViewModel)
        {
            var findedcategory = _onlineShoppingDbContext.ProductCategories.FirstOrDefault(s => s.Id == updateViewModel.Id);
            if (findedcategory == null) return NotFound();

            findedcategory.Title = updateViewModel.Title;

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");

        }

        public IActionResult Delete (int? id)
        {
            if(id == null) return NotFound();   

            var findedcategory = _onlineShoppingDbContext.ProductCategories.FirstOrDefault(s => s.Id == id);

            if (findedcategory == null) return NotFound();

            _onlineShoppingDbContext.ProductCategories.Remove(findedcategory);

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");

        }
    }
}
