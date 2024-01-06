using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Areas.Admin.ViewModels.Deal;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;
using OnlineShoppingApp.Extension;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class DealController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DealController(OnlineShoppingDbContext onlineShoppingDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View(_onlineShoppingDbContext.Deals.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel dealModel)
        {
            if (!ModelState.IsValid)
            {
                return View(dealModel);
            }
            if (!dealModel.Photo.CheckImage())
            {
                ModelState.AddModelError("Photo", "Add only photo");
                return View();
            }

            if (dealModel.Photo.CheckImageSize(1000))
            {
                ModelState.AddModelError("Photo", "Size is high");
                return View();
            }

            var newDeal = new Deal
            {
                Title = dealModel.Title,
                Description = dealModel.Description,
                Offer = dealModel.Offer,
                Url = dealModel.Url
            };

            string fileName = Guid.NewGuid() + dealModel.Photo.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets" , "img","deal", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                dealModel.Photo.CopyTo(stream);

            };

            newDeal.ImagePath = fileName;

            _onlineShoppingDbContext.Deals.Add(newDeal);

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var findedDeal = _onlineShoppingDbContext.Deals.FirstOrDefault(f => f.Id == id);
            if (findedDeal == null) return NotFound();

            var updateViewModel = new UpdateViewModel
            {
                Id = findedDeal.Id,
                Title = findedDeal.Title,
                Description = findedDeal.Description,
                Offer = findedDeal.Offer,
                Url = findedDeal.Url,
                ImagePath = findedDeal.ImagePath
            };

            return View(updateViewModel);
        }

        [HttpPost]
        public IActionResult Update(UpdateViewModel updateViewModel)
        {
            var findedDeal = _onlineShoppingDbContext.Deals.FirstOrDefault(f => f.Id == updateViewModel.Id);
            if (findedDeal == null) return NotFound();

            findedDeal.Title = updateViewModel.Title;
            findedDeal.Offer = updateViewModel.Offer;
            findedDeal.Description = findedDeal.Description;
            findedDeal.Url = findedDeal.Url;

            if(updateViewModel.Photo != null)
            {
                if (!updateViewModel.Photo.CheckImage())
                {
                    ModelState.AddModelError("Photo", "Only Photo.");
                    return View(updateViewModel);
                }

                if (updateViewModel.Photo.CheckImageSize(1000))
                {
                    ModelState.AddModelError("Photo", "Size is high.");
                    return View(updateViewModel);
                }

                var imagePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "deal", findedDeal.ImagePath);
                if (System.IO.File.Exists(imagePathToDelete))
                {
                    System.IO.File.Delete(imagePathToDelete);
                }

                string fileName = Guid.NewGuid() + updateViewModel.Photo.FileName;
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "deal", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    updateViewModel.Photo.CopyTo(stream);

                };

                findedDeal.ImagePath = fileName;
            }

            return RedirectToAction("List");

        }

        [HttpPost]

        public IActionResult Delete (int? id)
        {
            if (id == null) return NotFound();
            var findedDeal = _onlineShoppingDbContext.Deals.FirstOrDefault(f => f.Id == id);
            if (findedDeal == null) return NotFound();

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "deal", findedDeal.ImagePath);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _onlineShoppingDbContext.Remove(findedDeal);

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");

        }
    }
}
