using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Areas.Admin.ViewModels.Size;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]

    public class SizeController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;

        public SizeController(OnlineShoppingDbContext onlineShoppingDbContext)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View(_onlineShoppingDbContext.Sizes.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Size size)
        {
            var newSize = new Size
            {
                Title = size.Title
            };

            _onlineShoppingDbContext.Sizes.Add(newSize);

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var findedSize = _onlineShoppingDbContext.Sizes.FirstOrDefault(s => s.Id == id);

            if (findedSize == null) return NotFound();

            var sizeUpdateViewModel = new UpdateViewModel
            {
                Id = findedSize.Id,
                Title = findedSize.Title
            };

            return View(sizeUpdateViewModel);
        }

        [HttpPost]
        public IActionResult Update(UpdateViewModel updateViewModel)
        {
            var findedSize = _onlineShoppingDbContext.Sizes.FirstOrDefault(s => s.Id == updateViewModel.Id);
            if (findedSize == null) return NotFound();

            findedSize.Title = updateViewModel.Title;

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");

        }

        public IActionResult Delete (int? id)
        {
            if(id == null) return NotFound();   

            var findedSize = _onlineShoppingDbContext.Sizes.FirstOrDefault(s => s.Id == id);

            if (findedSize == null) return NotFound();

            _onlineShoppingDbContext.Sizes.Remove(findedSize);

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");

        }
    }
}
