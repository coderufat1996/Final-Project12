using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]

    public class SubscriberController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;

        public SubscriberController(OnlineShoppingDbContext onlineShoppingDbContext)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View(_onlineShoppingDbContext.Subscribers.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Subscriber subscriber)
        {
            if (!ModelState.IsValid) return View();

            var newSubscriber = new Subscriber
            {
                Email = subscriber.Email
            };

            _onlineShoppingDbContext.Subscribers.Add(newSubscriber);
            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if(id == null) return NotFound();
            var findedSubscriber = _onlineShoppingDbContext.Subscribers.FirstOrDefault(s => s.Id == id);
            if (findedSubscriber == null) return NotFound();
            return View(findedSubscriber);
        }

        [HttpPost]
        public IActionResult Update(Subscriber subscriber)
        {
            var findedSubscriber = _onlineShoppingDbContext.Subscribers.FirstOrDefault(s => s.Id == subscriber.Id);
            if (findedSubscriber == null) return NotFound();

            findedSubscriber.Email = subscriber.Email;

            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var findedSubscriber = _onlineShoppingDbContext.Subscribers.FirstOrDefault(s => s.Id == id);
            if (findedSubscriber == null) return NotFound();

            _onlineShoppingDbContext.Remove(findedSubscriber);


            _onlineShoppingDbContext.SaveChanges();

            return RedirectToAction("List");
        }
    }
}
