using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Areas.Admin.ViewModels.Contact;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class ContactController : Controller
    {
        private readonly OnlineShoppingDbContext _onlineShoppingDbContext;

        public ContactController(OnlineShoppingDbContext onlineShoppingDbContext)
        {
            _onlineShoppingDbContext = onlineShoppingDbContext;
        }
        [HttpGet]
        public IActionResult Update()
        {
            var contact = _onlineShoppingDbContext.Contacts.FirstOrDefault();
            if (contact == null)
            {
                return View(new UpdateViewModel());
            }

            var contactUpdateViewModel = new UpdateViewModel
            {
                Tittle = contact.Tittle,
                AddresTittle = contact.AddresTittle,
                AddresLine = contact.AddresLine,
                Email = contact.Email,
                Telephone = contact.Telephone,
                WorkTime = contact.WorkTime,
            };

            return View(contactUpdateViewModel);
        }

        [HttpPost]
        public IActionResult Update(UpdateViewModel contactUpdateViewModel)
        {
            var contact = _onlineShoppingDbContext.Contacts.FirstOrDefault();

            if (contact == null)
            {
                var newcontact = new Contact
                {
                    Tittle = contactUpdateViewModel.Tittle,
                    AddresTittle = contactUpdateViewModel.AddresTittle,
                    AddresLine = contactUpdateViewModel.AddresLine,
                    Email = contactUpdateViewModel.Email,
                    Telephone = contactUpdateViewModel.Telephone,
                    WorkTime = contactUpdateViewModel.WorkTime,

                };

                _onlineShoppingDbContext.Contacts.Add(newcontact);
            }

            else
            {
                contact.Tittle = contactUpdateViewModel.Tittle;
                contact.AddresTittle = contactUpdateViewModel.AddresTittle;
                contact.AddresLine = contactUpdateViewModel.AddresLine;
                contact.Email = contactUpdateViewModel.Email;
                contact.Telephone = contactUpdateViewModel.Telephone;
                contact.WorkTime = contactUpdateViewModel.WorkTime;
            }

            _onlineShoppingDbContext.SaveChanges();
            return RedirectToAction("update");
        }
    }
}
