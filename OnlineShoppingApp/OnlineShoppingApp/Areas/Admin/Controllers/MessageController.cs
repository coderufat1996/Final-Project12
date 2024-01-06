using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]

    public class MessageController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;

        [HttpGet]
        public IActionResult List()
        {
            var messages = _onlineShoppingDbContext.Messages.ToList();
            return View(messages);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Message message)
        {
            var newMessage = new Message
            {
                Name = message.Name,
                Subject = message.Subject,
                Content = message.Content,
                Email = message.Email
            };
            _onlineShoppingDbContext.Messages.Add(newMessage);
            _onlineShoppingDbContext.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var message = _onlineShoppingDbContext.Messages.FirstOrDefault(m => m.Id == id);
            if(message == null) return NotFound();
            return View(message);
        }

        [HttpPost]
        public IActionResult Update(Message message)
        {
            var findedMessage = _onlineShoppingDbContext.Messages.FirstOrDefault(m => m.Id == message.Id);
            if (message == null) return NotFound();

            findedMessage.Name = message.Name;
            findedMessage.Subject = message.Subject;
            findedMessage.Content = message.Content;
            findedMessage.Email = message.Email;

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");
        }
    }
}
