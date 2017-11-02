using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Lesson11.Models;
using Microsoft.AspNetCore.Authorization;
using Lesson11.ViewModels;
using System.Threading;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lesson11.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string
       returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe,
                lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
            }
          
            // If there is any error, display the form again 
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        static volatile bool adminExists;
        static readonly SemaphoreSlim adminSemaphore = new SemaphoreSlim(1);

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            const string adminRoleName = "Admin";
            const string adminUserName = "admin";

            async Task addUserToAdminRole(ApplicationUser user)
            {
                //To ensure singleton we use double check lock pattern.
                if(!adminExists)
                {
                    //because we are in async context instead of regular locks we use semaphore.waitasync
                    // as awaits are not allowed in regular locks.
                    await adminSemaphore.WaitAsync();
                    try
                    {
                        adminExists = await _roleManager.RoleExistsAsync(adminRoleName);
                        if (!adminExists)
                        {
                            //we are sure that the role does not exist so we create one.
                            var role = new IdentityRole(adminRoleName);
                            var result = await _roleManager.CreateAsync(role);
                            if (result.Succeeded)
                            {
                                adminExists = true;
                            }
                            else
                            {
                                throw new InvalidOperationException("role creation failed");
                            }
                        }
                    }
                    finally
                    {
                        //we release the lock.
                        adminSemaphore.Release();
                    }
                }
                //at this point we are sure the role exists so lets associate it with the user.
                await _userManager.AddToRoleAsync(user, adminRoleName);
            }


            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user,
                model.Password);
                if (result.Succeeded)
                {

                    await _signInManager.SignInAsync(user, isPersistent:
                    false);

                    if(user.UserName.StartsWith(adminUserName, StringComparison.OrdinalIgnoreCase))
                    {
                        await addUserToAdminRole(user);
                    }
                    return RedirectToAction(nameof(HomeController.Index),
                    "Home");
                }
                foreach (var error in result.Errors.Select(c => c.Description))
                {
                    ModelState.AddModelError("", error);
                }


            }

            

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }



    }


}
