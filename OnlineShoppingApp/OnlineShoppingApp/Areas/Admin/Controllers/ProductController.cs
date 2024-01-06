using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Areas.Admin.ViewModels.Product;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;
using OnlineShoppingApp.Extension;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("admin")]

    public class ProductController : Controller
    {
        public readonly OnlineShoppingDbContext _onlineShoppingDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(OnlineShoppingDbContext onlineShoppingDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View(_onlineShoppingDbContext.Products
                .Include(p => p.ProductCategory).ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            var productCreateViewModel = new ProductCreateViewModel
            {
                ProductCategories = _onlineShoppingDbContext.ProductCategories.Select(p => new ProductCategoryViewModel(p.Id, p.Title)).ToList(),
                ProductSizes = _onlineShoppingDbContext.Sizes.Select(p => new ProductSizeViewModel(p.Id, p.Title)).ToList()
            };
            return View(productCreateViewModel);
        }

        [HttpPost]
        public IActionResult Create(ProductCreateViewModel productCreateViewModel)
        {
            if (!ModelState.IsValid) return View();
            if (!productCreateViewModel.MainImage.CheckImage())
            {
                ModelState.AddModelError("Photo", "Add only photo");
                return View();
            }

            if (productCreateViewModel.MainImage.CheckImageSize(1000))
            {
                ModelState.AddModelError("Photo", "Size is high");
                return View();
            }

            if (!CheckSize(productCreateViewModel.SizeIds))
            {
                return View(productCreateViewModel);
            }

            foreach (var iamge in productCreateViewModel.Images)
            {
                if (!iamge.CheckImage())
                {
                    ModelState.AddModelError("Photo", "Add only photo");
                    return View();
                }

                if (iamge.CheckImageSize(1000))
                {
                    ModelState.AddModelError("Photo", "Size is high");
                    return View();
                }
            }

            var product = new Product
            {
                Name = productCreateViewModel.Name,
                Price = productCreateViewModel.Price,
                Description = productCreateViewModel.Description,
                Rate = productCreateViewModel.Rate,
                CreateTime = DateTime.Now,
                ProductCategoryId = (int)productCreateViewModel.ProductCategoryId,
                MainImagePath = GenerateImagePath(productCreateViewModel.MainImage)
            };

            foreach (var image in productCreateViewModel.Images)
            {
                var productImage = new ProductImage
                {
                    Product = product,
                    ImagePath = GenerateImagePath(image)
                };

                _onlineShoppingDbContext.ProductImages.Add(productImage);
            }

            foreach (var sizeId in productCreateViewModel.SizeIds)
            {
                var productSize = new ProductSize
                {
                    Product = product,
                    SizeId = sizeId
                };

                _onlineShoppingDbContext.ProductSizes.Add(productSize);
            }

            _onlineShoppingDbContext.Products.Add(product);
            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var product = _onlineShoppingDbContext.Products.Include(p => p.ProductCategory).Include(p => p.ProductSizes).FirstOrDefault(x => x.Id == id);
            if (product == null) return NotFound();
            var updateViewModel = new UpdateViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Rate = product.Rate,
                MainImagePath = product.MainImagePath,
                ProductCategories = _onlineShoppingDbContext.ProductCategories.Select(p => new ProductCategoryViewModel(p.Id, p.Title)).ToList(),
                ProductSizes = _onlineShoppingDbContext.Sizes.Select(p => new ProductSizeViewModel(p.Id, p.Title)).ToList(),
                SizeIds = product.ProductSizes.Select(ps => ps.SizeId).ToList(),
                ProductCategoryId = product.ProductCategoryId,
            };

            return View(updateViewModel);
        }

        [HttpPost]
        public IActionResult Update(UpdateViewModel updateViewModel)
        {
            if (!ModelState.IsValid) return View();


            if (!CheckSize(updateViewModel.SizeIds))
            {
                return View(updateViewModel);
            }

            var product = _onlineShoppingDbContext.Products
                .Include(p => p.ProductSizes)
                .Include(p => p.ProductCategory)
                .FirstOrDefault(x => x.Id == updateViewModel.Id);
            if (product == null) return NotFound();

            product.Name = updateViewModel.Name;
            product.Description = updateViewModel.Description;
            product.Price = updateViewModel.Price;
            product.ProductCategoryId = (int)updateViewModel.ProductCategoryId;
            product.Rate = updateViewModel.Rate;
            UpdateProductSize();


            if (updateViewModel.MainImage != null)
            {
                if (!updateViewModel.MainImage.CheckImage())
                {
                    ModelState.AddModelError("Photo", "Only Photo.");
                    return View(updateViewModel);
                }

                if (updateViewModel.MainImage.CheckImageSize(1000))
                {
                    ModelState.AddModelError("Photo", "Size is high.");
                    return View(updateViewModel);
                }

                var imagePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "products", product.MainImagePath);
                if (System.IO.File.Exists(imagePathToDelete))
                {
                    System.IO.File.Delete(imagePathToDelete);
                }

                product.MainImagePath = GenerateImagePath(updateViewModel.MainImage);
            }

            void UpdateProductSize()
            {
                var sizesInDb = product!.ProductSizes.Select(p => p.SizeId).ToList();

                var sizesToRemove = sizesInDb.Except(updateViewModel.SizeIds).ToList();

                var sizesToAdd = updateViewModel.SizeIds.Except(sizesInDb).ToList();

                product.ProductSizes.RemoveAll(ps => sizesToRemove.Contains(ps.SizeId));

                foreach (var sizeId in sizesToAdd)
                {
                    var size = new ProductSize
                    {
                        SizeId = sizeId,
                        ProductId = product.Id
                    };

                    _onlineShoppingDbContext.ProductSizes.Add(size);
                }
            }

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var product = _onlineShoppingDbContext.Products.FirstOrDefault(x => x.Id == id);
            if (product == null) return NotFound();
            string imagePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "products", product.MainImagePath);
            if (System.IO.File.Exists(imagePathToDelete))
            {
                System.IO.File.Delete(imagePathToDelete);
            }

            _onlineShoppingDbContext.Remove(product);
            _onlineShoppingDbContext.SaveChanges();
            return RedirectToAction("List");
        }
        private bool CheckSize(List<int> SizeIds)
        {
            var sizeExists = Enumerable.SequenceEqual(_onlineShoppingDbContext.Sizes.Select(s => s.Id).ToList(), SizeIds);

            foreach (var sizeId in SizeIds)
            {
                if (!_onlineShoppingDbContext.Sizes.Any(c => c.Id == sizeId))
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong");
                    return false;
                }

            }
            return true;
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
