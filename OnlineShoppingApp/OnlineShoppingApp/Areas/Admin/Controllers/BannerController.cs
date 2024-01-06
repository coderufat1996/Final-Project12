using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Areas.Admin.ViewModels.Banner;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;
using OnlineShoppingApp.DAL.Entities.Enums;
using OnlineShoppingApp.Extension;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]

    public class BannerController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BannerController(OnlineShoppingDbContext onlineShoppingDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult List()
        {
            var banners = _onlineShoppingDbContext.Banners.Select(b => new ListViewModel
            {
                Id = b.Id,
                Tittle = b.Tittle,
                Description = b.Description,
                ImagePath = b.ImagePath,
                BannerPageType = BannerTypeExecute.GetBannerType(b.BannerPageType)
            }).ToList();
            return View(banners);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel createViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createViewModel);
            }
            if (!createViewModel.Photo.CheckImage())
            {
                ModelState.AddModelError("Photo", "Add only photo");
                return View();
            }

            if (createViewModel.Photo.CheckImageSize(1000))
            {
                ModelState.AddModelError("Photo", "Size is high");
                return View();
            }

            var banner = new Banner
            {
                Tittle = createViewModel.Tittle,
                BannerPageType = createViewModel.BannerPageType,
                Description = createViewModel.Description,
            };
            string fileName = Guid.NewGuid() + createViewModel.Photo.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath,"assets" ,"img", "banner", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                createViewModel.Photo.CopyTo(stream);

            };

            banner.ImagePath = fileName;
            _onlineShoppingDbContext.Banners.Add(banner);
            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var banner = _onlineShoppingDbContext.Banners.FirstOrDefault(b => b.Id == id);
            if (banner == null) return NotFound();

            var bannerUpdate = new UpdateViewModel
            {
                Id = banner.Id,
                Tittle = banner.Tittle,
                Description = banner.Description,
                ImagePath = banner.ImagePath,
                BannerPageType = banner.BannerPageType,
            };

            return View(bannerUpdate);
        }

        [HttpPost]
        public IActionResult Update(UpdateViewModel updateViewModel)
        {
            if (!ModelState.IsValid) return View();

            var banner = _onlineShoppingDbContext.Banners.FirstOrDefault(b => b.Id == updateViewModel.Id);
            if (banner == null) return NotFound();

            banner.Tittle = updateViewModel.Tittle;
            banner.Description = updateViewModel.Description;
            banner.BannerPageType = updateViewModel.BannerPageType;

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

                string imagePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "banner", banner.ImagePath);
                if (System.IO.File.Exists(imagePathToDelete))
                {
                    System.IO.File.Delete(imagePathToDelete);
                }

                string fileName = Guid.NewGuid() + updateViewModel.Photo.FileName;
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "banner", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    updateViewModel.Photo.CopyTo(stream);

                };

                banner.ImagePath = fileName;
            }
            _onlineShoppingDbContext.SaveChanges();


            return RedirectToAction("List");

        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var banner = _onlineShoppingDbContext.Banners.FirstOrDefault(b => b.Id == id);
            if (banner == null) return NotFound();
            string imagePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "banner", banner.ImagePath);
            if (System.IO.File.Exists(imagePathToDelete))
            {
                System.IO.File.Delete(imagePathToDelete);
            }

            _onlineShoppingDbContext.Remove(banner);
            _onlineShoppingDbContext.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
