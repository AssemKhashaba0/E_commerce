using E_commerce.Models;
using E_commerce.utility;
using E_commerce.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_commerce.Controllers
{
    public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager) : Controller
    {
        public async  Task<IActionResult> register()
        {
            if (!await roleManager.RoleExistsAsync(SD.CustomerRole))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.CustomerRole));
            }

            if (!await roleManager.RoleExistsAsync(SD.AdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.AdminRole));
            }

            if (!await roleManager.RoleExistsAsync(SD.companyRole))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.companyRole));
            }

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> register(ApplicationUserVM userVM)

        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(userVM.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "The email is already registered.");
                    return View(userVM);
                }
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    UserName = userVM.Name,
                    Email = userVM.Email,
                    City = userVM.City,
                    PhoneNumber = userVM.Phone
                };
                var result = await userManager.CreateAsync(applicationUser, userVM.Password);
                if (result.Succeeded)
                {
                 await userManager.AddToRoleAsync(applicationUser , SD.CustomerRole);
                    await signInManager.SignInAsync(applicationUser, false);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Password", "Invalid Password");
            }
            return View(userVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM userVM)

        {
            if (ModelState.IsValid)
            {
                var userDb =await userManager.FindByNameAsync(userVM.Username);
                if (userDb != null)
                {
                    var finalResult = await userManager.CheckPasswordAsync(userDb, userVM.Password);
                    if (finalResult)
                    {
                        await signInManager.SignInAsync(userDb, userVM.RememberMe);
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ModelState.AddModelError("password", "Invaild password");
                    }
                }
                else
                {
                    ModelState.AddModelError("Username", "Invaild Username");

                }
                
            }
            return View(userVM);


        }


        public IActionResult LogOut()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }

       


    }
}
