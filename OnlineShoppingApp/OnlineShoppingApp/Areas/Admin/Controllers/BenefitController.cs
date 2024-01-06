using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Areas.Admin.ViewModels.Benefit;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;
using OnlineShoppingApp.Extension;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class BenefitController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BenefitController(OnlineShoppingDbContext onlineShoppingDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View(_onlineShoppingDbContext.Benefits.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel benefitModel)
        {
            if (!ModelState.IsValid)
            {
                return View(benefitModel);
            }
            if (!benefitModel.Photo.CheckImage())
            {
                ModelState.AddModelError("Photo", "Add only photo");
                return View();
            }

            if (benefitModel.Photo.CheckImageSize(1000))
            {
                ModelState.AddModelError("Photo", "Size is high");
                return View();
            }

            var newbenefit = new Benefit
            {
                Title = benefitModel.Title,
            };

            string fileName = Guid.NewGuid() + benefitModel.Photo.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath,"assets","img", "benefit", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                benefitModel.Photo.CopyTo(stream);

            };

            newbenefit.ImagePath = fileName;

            _onlineShoppingDbContext.Benefits.Add(newbenefit);

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var findedbenefit = _onlineShoppingDbContext.Benefits.FirstOrDefault(f => f.Id == id);
            if (findedbenefit == null) return NotFound();

            var updateViewModel = new UpdateViewModel
            {
                Id = findedbenefit.Id,
                Title = findedbenefit.Title,
                ImagePath = findedbenefit.ImagePath
            };

            return View(updateViewModel);
        }

        [HttpPost]
        public IActionResult Update(UpdateViewModel updateViewModel)
        {
            var findedbenefit = _onlineShoppingDbContext.Benefits.FirstOrDefault(f => f.Id == updateViewModel.Id);
            if (findedbenefit == null) return NotFound();

            findedbenefit.Title = updateViewModel.Title;

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

                var imagePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "benefit", findedbenefit.ImagePath);
                if (System.IO.File.Exists(imagePathToDelete))
                {
                    System.IO.File.Delete(imagePathToDelete);
                }

                string fileName = Guid.NewGuid() + updateViewModel.Photo.FileName;
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    updateViewModel.Photo.CopyTo(stream);

                };

                findedbenefit.ImagePath = fileName;
            }

            return RedirectToAction("List");

        }

        [HttpPost]

        public IActionResult Delete (int? id)
        {
            if (id == null) return NotFound();
            var findedbenefit = _onlineShoppingDbContext.Benefits.FirstOrDefault(f => f.Id == id);
            if (findedbenefit == null) return NotFound();

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "benefit", findedbenefit.ImagePath);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _onlineShoppingDbContext.Remove(findedbenefit);

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");

        }
    }
}
