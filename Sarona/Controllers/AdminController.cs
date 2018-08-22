using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sarona.Models;
using System.Threading.Tasks;
using Sarona.Models;
using Microsoft.AspNetCore.Authorization;

namespace Sarona.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        public AdminController(UserManager<AppUser> usrMgr)
        {
            userManager = usrMgr;
        }
        public ViewResult Users() => View(userManager.Users);

        public ViewResult CreateUser() => View();
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUser model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Username,
                    Email = model.Username + "@local"
                };
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

    }
}
