using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;

        public ContactController(OnlineShoppingDbContext onlineShoppingDbContext)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var contactViewModel = new ContactViewModel
            {
                Banner = _onlineShoppingDbContext.Banners.FirstOrDefault(b => b.BannerPageType == DAL.Entities.Enums.BannerPageType.Contact),
                Contact = _onlineShoppingDbContext.Contacts.FirstOrDefault(),

            };
            return View(contactViewModel);
        }

        [HttpPost]
        public IActionResult SendMessage(Message message)
        {
            var newMesage = new Message
            {
                Name = message.Name,
                Email = message.Email,
                Subject = message.Subject,
                Content = message.Content,
            };

            _onlineShoppingDbContext.Messages.Add(message);
            _onlineShoppingDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
