using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Areas.Admin.ViewModels.Blog;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;
using OnlineShoppingApp.Extension;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class BlogController : Controller
    {
        public readonly OnlineShoppingDbContext _onlineShoppingDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(OnlineShoppingDbContext onlineShoppingDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult List()
        {
            return View(_onlineShoppingDbContext.Blogs.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateViewModel()); 
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel createViewModel)
        {
            if (!ModelState.IsValid) return View();
            if (!createViewModel.MainImage.CheckImage())
            {
                ModelState.AddModelError("Photo", "Add only photo");
                return View();
            }

            if (createViewModel.MainImage.CheckImageSize(1000))
            {
                ModelState.AddModelError("Photo", "Size is high");
                return View();
            }

            foreach (var iamge in createViewModel.Images)
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


            var blog = new Blog
            {
                Tittle = createViewModel.Tittle,
                Author = createViewModel.Author,
                Content = createViewModel.Content,
                MainImagePath = GenerateImagePath(createViewModel.MainImage),
                CreateTime = DateTime.Now
                
            };

            foreach (var image in createViewModel.Images)
            {
                var blogImage = new BlogImage
                {
                    Blog = blog,
                    ImagePath = GenerateImagePath(image)
                };

                _onlineShoppingDbContext.BlogImages.Add(blogImage);
            }

            _onlineShoppingDbContext.Blogs.Add(blog);

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var findedBlog = _onlineShoppingDbContext.Blogs.FirstOrDefault(b => b.Id == id);
            if (findedBlog == null) return NotFound();
            var updateViewModel = new UpdateViewModel
            {
                Id = findedBlog.Id,
                Tittle = findedBlog.Tittle,
                Author = findedBlog.Author,
                Content = findedBlog.Content,
                MainImagePath = findedBlog.MainImagePath,
                
            };

            return View(updateViewModel);
        }

        [HttpPost]
        public IActionResult Update(UpdateViewModel updateViewModel)
        {
            if (!ModelState.IsValid) return View();

            if (updateViewModel.Id == null) return NotFound();
            var findedBlog = _onlineShoppingDbContext.Blogs.FirstOrDefault(b => b.Id == updateViewModel.Id);
            if (findedBlog == null) return NotFound();

            findedBlog.Tittle = updateViewModel.Tittle;
            findedBlog.Author = updateViewModel.Author;
            findedBlog.Content = updateViewModel.Content;

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

                var imagePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "blog", findedBlog.MainImagePath);
                if (System.IO.File.Exists(imagePathToDelete))
                {
                    System.IO.File.Delete(imagePathToDelete);
                }

                findedBlog.MainImagePath = GenerateImagePath(updateViewModel.MainImage);
            }

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("list");
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var findedBlog = _onlineShoppingDbContext.Blogs.FirstOrDefault(b => b.Id == id);
            if (findedBlog == null) return NotFound();
            string imagePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "blog", findedBlog.MainImagePath);
            if (System.IO.File.Exists(imagePathToDelete))
            {
                System.IO.File.Delete(imagePathToDelete);
            }

            _onlineShoppingDbContext.Remove(findedBlog);
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
