using OnlineShoppingApp.Areas.Admin.ViewModels.UserVM;
using OnlineShoppingApp.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShoppingApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> List()
        {
            var users = _userManager.Users.ToList();

            return View(users);
        }

        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel userCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = userCreateViewModel.UserName,
                    Email = userCreateViewModel.Email,
                    FullName = userCreateViewModel.FullName
                };

                var result = await _userManager.CreateAsync(user, userCreateViewModel.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(userCreateViewModel);

        }


        public async Task<IActionResult> Update(string id)
        {
            if (id == null) return NotFound();


            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            UserUpdateViewModel editUserVM = new()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName
            };

            return View(editUserVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateViewModel updateViewModel)
        {
            if (!ModelState.IsValid) return View(updateViewModel);
            var user = await _userManager.FindByIdAsync(updateViewModel.Id);

            if (user == null) return NotFound();

            user.UserName = updateViewModel.UserName;
            user.Email = updateViewModel.Email;
            user.FullName = updateViewModel.FullName;

            var isOldPasswordValid = await _userManager.CheckPasswordAsync(user, updateViewModel.OldPassword);

            if (!isOldPasswordValid)
            {
                ModelState.AddModelError("", "Old password is wrong");
                return View(updateViewModel);
            }

            if (!string.IsNullOrEmpty(updateViewModel.NewPassword))
            {
                var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, updateViewModel.NewPassword);
                user.PasswordHash = newPasswordHash;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(updateViewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);

            return RedirectToAction("index");
        }
    }
}
