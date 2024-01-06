using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Areas.Admin.ViewModels.ProductImage;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;
using OnlineShoppingApp.Extension;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]

    public class ProductImageController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductImageController(OnlineShoppingDbContext onlineShoppingDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult List(int productId)
        {
            var listItemViewModel = new ListViewModel
            {
                ProductId = productId,
                Images = _onlineShoppingDbContext.ProductImages.Where(p => p.ProductId == productId)
                    .Select(pi => new ItemViewModel(pi.Id, pi.ImagePath)).ToList(),

            };
          
            return View(listItemViewModel);
        }

        [HttpGet]
        public IActionResult Create(int productId)
        {
            var creatViewModel = new CreateViewModel
            {
                productId = productId
            };

            return View(creatViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel createViewModel, int productId)
        {
            if (!ModelState.IsValid)
            {
                return View(createViewModel);
            }

            if(!_onlineShoppingDbContext.Products.Any(p => p.Id == productId))
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

            var productImage = new ProductImage
            {
                ProductId = productId,
                ImagePath = createViewModel.Photo != null ? GenerateImagePath(createViewModel.Photo) : default
            };

            _onlineShoppingDbContext.ProductImages.Add(productImage);
            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List", new {productId = productId});

        }

        [HttpGet]
        public IActionResult Update(int? productId , int? id)
        {
            if (productId == null & id == null) return NotFound();

            var ProductImage = _onlineShoppingDbContext.ProductImages
                .FirstOrDefault(pi => pi.Id == id && pi.ProductId == productId);

            if(ProductImage == null) return NotFound();

            var UpdateViewModel = new UpdateViewModel
            {
                Id = ProductImage.Id,
                ProductId = ProductImage.ProductId,
                ImagePath = ProductImage.ImagePath
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

            if (updateViewModel.ProductId == null & updateViewModel.Id == null) return NotFound();

            var ProductImage = _onlineShoppingDbContext.ProductImages
                .FirstOrDefault(pi => pi.Id == updateViewModel.Id && pi.ProductId == updateViewModel.ProductId);

            if (ProductImage == null) return NotFound();

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

                var imagePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "products", ProductImage.ImagePath);
                if (System.IO.File.Exists(imagePathToDelete))
                {
                    System.IO.File.Delete(imagePathToDelete);
                }

                ProductImage.ImagePath = GenerateImagePath(updateViewModel.Photo);
            }

            _onlineShoppingDbContext.SaveChanges();
            return RedirectToAction("List", new { productId = updateViewModel.ProductId });

        }

        [HttpPost]
        public IActionResult Delete(int? productId, int? id)
        {
            if (productId == null & id == null) return NotFound();

            var ProductImage = _onlineShoppingDbContext.ProductImages
                .FirstOrDefault(pi => pi.Id == id && pi.ProductId == productId);

            if (ProductImage == null) return NotFound();

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "products", ProductImage.ImagePath);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _onlineShoppingDbContext.Remove(ProductImage);

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");

        }
        private string GenerateImagePath(IFormFile file)
        {

            string fileName = Guid.NewGuid() + file.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "products", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);

            };
            return fileName;
        }
    }
}
