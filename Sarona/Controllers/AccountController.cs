using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sarona.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sarona.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;


        public AccountController(UserManager<AppUser> usrMgr,
                IUserValidator<AppUser> userValid,
                IPasswordValidator<AppUser> passValid,
                IPasswordHasher<AppUser> passwordHash,
                SignInManager<AppUser> signinMgr)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
            userValidator = userValid;
            passwordValidator = passValid;
            signInManager = signinMgr;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldPass, string newPass)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var validPass = await passwordValidator.ValidateAsync(userManager, user, newPass);
            if (validPass.Succeeded)
            {
                 
                if (await userManager.CheckPasswordAsync(user,oldPass))
                {
                    await userManager.ChangePasswordAsync(user, oldPass, newPass);
                    return Json(true);
                }
                return Json("Wrong password!!!");
            }
            else
            {
                string error = "";
                foreach (var e in validPass.Errors)
                {
                    error = error + e.Description + Environment.NewLine;
                }
                return Json(error);
                
            }
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> IsUsernameUnique(string Username)
        {
            AppUser user = await userManager.FindByNameAsync(Username);
            if (user is null)
                return Json(true);
            else
                return Json("There is a user with same username.");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(details.Username);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                            await signInManager.PasswordSignInAsync(user, details.Password, details.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Username), "Invalid username or password");
            }
            return View(details);
        }

        
        




    }

}
