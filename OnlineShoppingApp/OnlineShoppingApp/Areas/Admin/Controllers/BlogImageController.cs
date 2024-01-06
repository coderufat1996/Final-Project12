using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Areas.Admin.ViewModels.BlogImage;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;
using OnlineShoppingApp.Extension;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogImageController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogImageController(OnlineShoppingDbContext onlineShoppingDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult List(int blogId)
        {
            var listItemViewModel = new ListViewModel
            {
                blogId = blogId,
                Images = _onlineShoppingDbContext.BlogImages.Where(p => p.BlogId == blogId)
                    .Select(bi => new ItemViewModel(bi.Id, bi.ImagePath)).ToList(),

            };

            return View(listItemViewModel);
        }

        [HttpGet]
        public IActionResult Create(int blogId)
        {
            var creatViewModel = new CreateViewModel
            {
                blogId = blogId
            };

            return View(creatViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel createViewModel, int blogId)
        {
            if (!ModelState.IsValid)
            {
                return View(createViewModel);
            }

            if (!_onlineShoppingDbContext.Blogs.Any(b => b.Id == blogId))
            {
                return NotFound();
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

            var blogImage = new BlogImage
            {
                BlogId = blogId,
                ImagePath = createViewModel.Photo != null ? GenerateImagePath(createViewModel.Photo) : default
            };

            _onlineShoppingDbContext.BlogImages.Add(blogImage);
            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List", new { blogId = blogId });

        }

        [HttpGet]
        public IActionResult Update(int? blogId, int? id)
        {
            if (blogId == null & id == null) return NotFound();

            var blogImage = _onlineShoppingDbContext.BlogImages
                .FirstOrDefault(bi => bi.Id == id && bi.BlogId == blogId);

            if (blogImage == null) return NotFound();

            var UpdateViewModel = new UpdateViewModel
            {
                Id = blogImage.Id,
                blogId = blogImage.BlogId,
                ImagePath = blogImage.ImagePath
            };

            return View(UpdateViewModel);
        }

        [HttpPost]
        public IActionResult Update(UpdateViewModel updateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateViewModel);
            }
            if (!updateViewModel.Photo.CheckImage())
            {
                ModelState.AddModelError("Photo", "Add only photo");
                return View();
            }

            if (updateViewModel.Photo.CheckImageSize(1000))
            {
                ModelState.AddModelError("Photo", "Size is high");
                return View();
            }

            if (updateViewModel.blogId == null & updateViewModel.Id == null) return NotFound();

            var blogImage = _onlineShoppingDbContext.BlogImages
                .FirstOrDefault(bi => bi.Id == updateViewModel.Id && bi.BlogId == updateViewModel.blogId);

            if (blogImage == null) return NotFound();

            if (updateViewModel.Photo != null)
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

                var imagePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "blog", blogImage.ImagePath);
                if (System.IO.File.Exists(imagePathToDelete))
                {
                    System.IO.File.Delete(imagePathToDelete);
                }

                blogImage.ImagePath = GenerateImagePath(updateViewModel.Photo);
            }

            _onlineShoppingDbContext.SaveChanges();
            return RedirectToAction("List", new { blogId = updateViewModel.blogId });

        }

        [HttpPost]
        public IActionResult Delete(int? blogId, int? id)
        {
            if (blogId == null & id == null) return NotFound();

            var blogImage = _onlineShoppingDbContext.BlogImages
                .FirstOrDefault(bi => bi.Id == id && bi.BlogId == blogId);

            if (blogImage == null) return NotFound();

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "blog", blogImage.ImagePath);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _onlineShoppingDbContext.Remove(blogImage);

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");

        }
        private string GenerateImagePath(IFormFile file)
        {

            string fileName = Guid.NewGuid() + file.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "blog", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);

            };
            return fileName;
        }
    }

}
