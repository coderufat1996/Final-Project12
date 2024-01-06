using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Areas.Admin.ViewModels.About;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;
using OnlineShoppingApp.Extension;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]

    public class AboutController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AboutController(OnlineShoppingDbContext onlineShoppingDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Update()
        {
            var about = _onlineShoppingDbContext.Abouts.FirstOrDefault();
            if (about == null)
            {
                return View(new AboutUpdateViewModel());
            }

            var aboutUpdateViewModel = new AboutUpdateViewModel
            {
                Tittle = about.Tittle,
                Description = about.Description,
                Offer = about.Offer,
                ImagePath = about.ImagePath,
            };

            return View(aboutUpdateViewModel);
        }

        [HttpPost]
        public IActionResult Update(AboutUpdateViewModel aboutUpdateViewModel)
        {
            var about = _onlineShoppingDbContext.Abouts.FirstOrDefault();

            if (about == null)
            {
                var newAbout = new About
                {
                    Tittle = aboutUpdateViewModel.Tittle,
                    Description = aboutUpdateViewModel.Description,
                    Offer = aboutUpdateViewModel.Offer

                };

                if (!aboutUpdateViewModel.Photo.CheckImage())
                {
                    ModelState.AddModelError("Photo", "Add only photo");
                    return View();
                }

                if (aboutUpdateViewModel.Photo.CheckImageSize(1000))
                {
                    ModelState.AddModelError("Photo", "Size is high");
                    return View();
                }

                string fileName = Guid.NewGuid() + aboutUpdateViewModel.Photo.FileName;
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "about", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    aboutUpdateViewModel.Photo.CopyTo(stream);

                };

                newAbout.ImagePath = fileName;

                _onlineShoppingDbContext.Abouts.Add(newAbout);
            }

            else
            {

                about.Tittle = about.Tittle;
                about.Description = aboutUpdateViewModel.Description;
                about.Offer = aboutUpdateViewModel.Offer;

                if (aboutUpdateViewModel.Photo != null)
                {
                    if (!aboutUpdateViewModel.Photo.CheckImage())
                    {
                        ModelState.AddModelError("Photo", "Only Photo.");
                        return View(aboutUpdateViewModel);
                    }

                    if (aboutUpdateViewModel.Photo.CheckImageSize(1000))
                    {
                        ModelState.AddModelError("Photo", "Size is high.");
                        return View(aboutUpdateViewModel);
                    }

                    var imagePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "about", about.ImagePath);
                    if (System.IO.File.Exists(imagePathToDelete))
                    {
                        System.IO.File.Delete(imagePathToDelete);
                    }

                    string fileName = Guid.NewGuid() + aboutUpdateViewModel.Photo.FileName;
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        aboutUpdateViewModel.Photo.CopyTo(stream);

                    };

                    about.ImagePath = fileName;

                }
            }

            _onlineShoppingDbContext.SaveChanges();
            return RedirectToAction("update");
        }

    }
}
