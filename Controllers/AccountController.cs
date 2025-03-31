using MediQuickFinal.Models;
using MediQuickFinal.Models.AccountViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace MediQuickFinal.Controllers
{
    public class AccountController(UserManager<MediUser> userManager) : Controller
    {
        private readonly UserManager<MediUser> _userManager = userManager;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register() 
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            if(!registerViewModel.Password.Equals(registerViewModel.ConfirmPassword))
            {
                ModelState.AddModelError("Password", "Passwords do not match");
                return View(registerViewModel);
            }
            MediUser user = new()
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email,
                MediRole = MediRole.Guest,

            };
            //Create the user
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
                    return View(registerViewModel);
            }
            else
            {
                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, registerViewModel.FullName),
                    new (ClaimTypes.Email, user.Email),
                    new (ClaimTypes.NameIdentifier, user.Id),
                    new (ClaimTypes.Role, "Guest"),

                };
                var claimResult = await _userManager.AddClaimsAsync(user, claims);
                await _userManager.UpdateAsync(user);
                if (!claimResult.Succeeded)
                {
                    foreach (var error in claimResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(registerViewModel);
                }
            }
            return View("Success");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string ReturnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt, user not found");
                return View(loginViewModel);
            }
            var result = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "invalid login attempt, password is incorrect");
                return View(loginViewModel);
            }
            else
            {
                var claims = user != null ? await _userManager.GetClaimsAsync(user) : null;
                if (claims != null)
                {
                    ViewData["ReturnUrl"] = ReturnUrl;
                    var scheme = IdentityConstants.ApplicationScheme;
                    var claimsIdentity = new ClaimsIdentity(claims, scheme);
                    var principle = new ClaimsPrincipal(claimsIdentity);
                    var authenticationProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
                    };
                    await HttpContext.SignInAsync(scheme, principle, authenticationProperties);
                    return Redirect(ReturnUrl);

                }

            }
            return View(loginViewModel);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Redirect("/Home/Index");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}

