using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using OnlineShoppingApp.DAL.Entities;
using OnlineShoppingApp.Servıces;
using OnlineShoppingApp.ViewModels;
using System.Net;
using System.Net.Mail;

namespace OnlineShoppingApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;


        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        public IActionResult Authentication()
        {
            return View(new AccountViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountViewModel accountViewModel)
        {
            ModelState.Remove(nameof(accountViewModel.LoginViewModel));

            if (!ModelState.IsValid) return RedirectToAction("Authentication");


            User user = new User
            {
                FullName = accountViewModel.RegisterViewModel.FullName,
                UserName = accountViewModel.RegisterViewModel.UserName,
                Email = accountViewModel.RegisterViewModel.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, accountViewModel.RegisterViewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction("Authentication", accountViewModel.RegisterViewModel);
            }

            await _userManager.AddToRoleAsync(user, "User");

            return RedirectToAction("Authentication");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Login(AccountViewModel accountViewModel, string? ReturnUrl)
        {
            ModelState.Remove(nameof(accountViewModel.RegisterViewModel));

            if (!ModelState.IsValid) return RedirectToAction("Authentication");

            User user = await _userManager.FindByEmailAsync(accountViewModel.LoginViewModel.UserNameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(accountViewModel.LoginViewModel.UserNameOrEmail);

                if (user == null)
                {
                    ModelState.AddModelError("", "Username and Email or password is incorrect");
                    return RedirectToAction("Authentication", accountViewModel.LoginViewModel);
                }

            }

            var result = await _signInManager.PasswordSignInAsync(user, accountViewModel.LoginViewModel.Password, accountViewModel.LoginViewModel.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Due to overtrying your account has been blocked for 5 minute");
                    return RedirectToAction("Authentication", accountViewModel.LoginViewModel);
                }

                ModelState.AddModelError("", "Username and Email or password is incorrect");
                return RedirectToAction("Authentication", accountViewModel.LoginViewModel);
            }

            await _signInManager.SignInAsync(user, accountViewModel.LoginViewModel.RememberMe);

            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                if (item == "Admin")
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
            }
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            User user = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("Error", "Qeyd etdiyiniz email tapilmadi.");
                return View();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token = token }, Request.Scheme, Request.Host.ToString());

            var subject = "Reset Password";
            var content = $"<a href={link}>Click Here to start reseting Password</a>";

            var message = new MessageViewModel(user.Email, subject, content);
           
            _emailService.Send(message);

            return RedirectToAction("index", "home");

        }
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();
            var resetModel = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };
            return View(resetModel);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string email, ResetPasswordViewModel forgetPasswordVM)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();
            //if (!ModelState.IsValid) return View();

            await _userManager.ResetPasswordAsync(user, token, forgetPasswordVM.Password);
            await _userManager.UpdateSecurityStampAsync(user);

            return RedirectToAction("Index", "Home");
        }


    }
}
