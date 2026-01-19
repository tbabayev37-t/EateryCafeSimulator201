using EateryCafeSimulator201.Models;
using EateryCafeSimulator201.ViewModels.UserViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EateryCafeSimulator201.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, RoleManager<IdentityRole> _roleManager, SignInManager<AppUser> _signInManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            AppUser user = new()
            {
                Fullname = vm.Username,
                Email = vm.Email,
                UserName = vm.Username
            };
            var userPassword = await _userManager.CreateAsync(user,vm.Password);
            if(!userPassword.Succeeded)
            {
                foreach (var error in userPassword.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    
                }
                return View(vm);
            }
            await _userManager.AddToRoleAsync(user, "Member");
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var Emailuser = await _userManager.FindByEmailAsync(vm.Email);
            if(Emailuser == null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(vm);
            }
            var PasswordUser = await _signInManager.PasswordSignInAsync(Emailuser, vm.Password, false, true);
            if(!PasswordUser.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(vm);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new() { Name = "Admin" });
            await _roleManager.CreateAsync(new() { Name = "Member" });
            await _roleManager.CreateAsync(new() { Name = "Moderator" });
            return Ok("Roles was created");
        }
    }
}
