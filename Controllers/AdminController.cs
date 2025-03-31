using MediQuickFinal.AppDbContext;
using MediQuickFinal.Models;
using MediQuickFinal.Models.AccountViewModel;
using MediQuickFinal.Models.AdminViewModel;
using MediQuickFinal.AppDbContext;

using MediQuickFinal.Models.AccountViewModel;


using MediQuickFinal.Models.AccountViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MediQuickFinal.Models.AdminViewModel;

namespace MediQuickFinal.Controllers
{
    //Authorize(Policy = "MustbeASuperAdmin")]
    public class AdminController(UserManager<MediUser> userManager, AppIdentityDbContext context) : Controller
    {
        private readonly UserManager<MediUser> _userManager = userManager;
        private readonly AppIdentityDbContext _dbContext = context;
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManageUsers()
        {
            return View(await GetUsersToManageAsync());
        }

        private async Task<List<UserViewModel>> GetUsersToManageAsync()
        {
            var User = await _userManager.Users
                .Where(x => x.MediRole != MediRole.SuperAdmin).ToListAsync();

            var listofUserAccounts = new List<UserViewModel>();
            foreach (var user in User)
            {
                listofUserAccounts.Add(new UserViewModel
                {
                    Email = user.Email,
                    Name = await GetNameForUser(user.Email ?? string.Empty),
                    Role = user.MediRole
                });
            }

            return listofUserAccounts;
        }

        private async Task<string> GetNameForUser(string Email)
        {
            var accountuser = await _userManager.FindByEmailAsync(Email);
            if (accountuser != null)
            {
                var claims = await _userManager.GetClaimsAsync(accountuser);
                if (claims != null)
                {
                    return claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
                }
            }
            return string.Empty;
        }

        public async Task<IActionResult> EditUser(string email)
        {
            var accountUser = await _userManager.FindByEmailAsync(email);

            if (accountUser != null)
            {
                UserViewModel userViewModel = new UserViewModel()
                {
                    Email = accountUser.Email,
                    Name = await GetNameForUser(accountUser.Email ?? string.Empty),
                    Role = accountUser.MediRole,
                    Roles = Enum.GetValues<MediRole>()
                        .Select(x => new SelectListItem
                        {
                            Text = Enum.GetName(x),
                            Value = x.ToString()
                        })
                };
                return View(userViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(userViewModel.Email))
                    {
                        MediUser? mediUser = await _userManager.FindByEmailAsync(userViewModel.Email);
                        if (mediUser != null)
                        {
                            mediUser.MediRole = userViewModel.Role;
                            var claims = await _userManager.GetClaimsAsync(mediUser);
                            var removeResult = await _userManager.RemoveClaimsAsync(mediUser, claims);

                            if (!removeResult.Succeeded)
                            {
                                ModelState.AddModelError(string.Empty, "Unable to update claim-removing existing claim");
                                return View(userViewModel);
                            }

                            var claimsRequired = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, userViewModel.Name ?? " "),
                                new Claim(ClaimTypes.Role, Enum.GetName(userViewModel.Role) ?? " "),
                                new Claim(ClaimTypes.NameIdentifier, mediUser.Id),
                                new Claim(ClaimTypes.Email, userViewModel.Email)
                            };
                            var addclaimResult = await _userManager.AddClaimsAsync(mediUser, claimsRequired);
                            if (!addclaimResult.Succeeded)
                            {
                                ModelState.AddModelError(string.Empty, "Unable to update claim - adding new claim failed");
                                return View(userViewModel);
                            }
                            var userUpdateResult = await _userManager.UpdateAsync(mediUser);
                            if (!userUpdateResult.Succeeded)
                            {
                                ModelState.AddModelError("", "Failed to update user");
                                return View(userViewModel);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(userViewModel);
                }
            }
            return RedirectToAction("ManageUsers", await GetUsersToManageAsync());
        }

        public async Task<IActionResult> DeleteUser(string email)
        {
            var accountUser = await _userManager.FindByEmailAsync(email);
            if (accountUser != null)
            {
                await _userManager.DeleteAsync(accountUser);
                return View("ManageUsers", await GetUsersToManageAsync());
            }
            return NotFound();
        }

        public IActionResult CreateUser()
        {
            return View(new CreateUserViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel createUserViewModel)

        {

            if (!ModelState.IsValid)

            {

                return View(createUserViewModel);

            }

            if (createUserViewModel.Email != createUserViewModel.ConfirmEmail)

            {

                ModelState.AddModelError(string.Empty, "Email and ConfirmEmail do not match");

                return View(createUserViewModel);

            }

            if (createUserViewModel.Password != createUserViewModel.ConfirmPassword)

            {

                ModelState.AddModelError(string.Empty, "Password and ConfirmPassword do not match");

                return View(createUserViewModel);

            }

            MediUser mediUser = new()

            {

                Email = createUserViewModel.Email,

                UserName = createUserViewModel.Name,

                MediRole = createUserViewModel.Role

            };

            var createdUser = await _userManager.CreateAsync(mediUser, createUserViewModel.Password ?? " ");

            if (!createdUser.Succeeded)

            {

                foreach (var error in createdUser.Errors)

                {

                    ModelState.AddModelError(string.Empty, error.Description);

                }

                return View(createUserViewModel);

            }

            var claimRequired = new List<Claim>

{

    new (ClaimTypes.Name, createUserViewModel.Name??" "),

    new (ClaimTypes.Role, Enum.GetName(createUserViewModel.Role) ?? string.Empty),

    new (ClaimTypes.NameIdentifier, mediUser.Id),

    new (ClaimTypes.Email, createUserViewModel.Email?? "")

};

            var claimResult = await _userManager.AddClaimsAsync(mediUser, claimRequired);

            await _userManager.UpdateAsync(mediUser);

            if (!claimResult.Succeeded)

            {

                foreach (var error in claimResult.Errors)

                {

                    ModelState.AddModelError(string.Empty, error.Description);

                }

                return View(createUserViewModel);

            }

            return RedirectToAction("ManageUsers", await GetUsersToManageAsync());
        }

        public async Task<IActionResult> ManageMedi()
        {
            var listOfMeds = await _dbContext.Medicines
                .Include(x => x.MediManager)
                .ToListAsync();
            List<MediViewModel> mediViewModels = listOfMeds.Select(x => new MediViewModel
            {
                MediId = x.MediId,
                MediName = x.MediName,
                MediPrice = x.MediPrice,
                MediCategory = x.MediCategory,
                MediType = x.MediType,
                MediManagerEmail = x.MediManager?.Email,
                MediDescription = x.MediDescription
            }).ToList();
            return View(mediViewModels);
        }

        public IActionResult CreateMedi()
        {
            return View(new MediViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedi(MediViewModel model, IFormFile MediBackground)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var medi = new Medicine
                {
                    MediName = model.MediName ?? "",
                    MediType = model.MediType,
                    MediCategory = model.MediCategory,
                    MediDescription = model.MediDescription,
                    MediManager = await _userManager.FindByEmailAsync(model.MediManagerEmail ?? string.Empty),
                };
                if(MediBackground != null)
                {
                    using var stream = new MemoryStream();
                    await MediBackground.CopyToAsync(stream);
                    medi.MediBackground = stream.ToArray();
                }
                _dbContext.Medicines.Add(medi);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("ManageMedi");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }


        }

        public async Task<IActionResult> EditMedi(int mediId)
        {
            var medi = await _dbContext
                        .Medicines
                        .Include(x => x.MediManager)
                        .FirstOrDefaultAsync(x => x.MediId == mediId);

            if (medi == null)
            {
                return NotFound();
            }

            var mediViewModel = new MediViewModel()
            {
                MediId = medi.MediId,
                MediName = medi.MediName,
                MediPrice = medi.MediPrice,
                MediBackground=medi.MediBackground,
                MediCategory = medi.MediCategory,
                MediType = medi.MediType,
                MediManagerEmail = medi.MediManager?.Email,
                MediDescription = medi.MediDescription,
            };
            

            return View(mediViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditMedi(MediViewModel mediViewModel,IFormFile MediBackground)
        {
            var medi = await _dbContext
                        .Medicines
                        .Include(x => x.MediManager)
                        .FirstOrDefaultAsync(x => x.MediId == mediViewModel.MediId);
            if (medi != null)
            {
                medi.MediName = mediViewModel.MediName ?? "";
                medi.MediCategory = mediViewModel.MediCategory;
                medi.MediPrice = mediViewModel.MediPrice;
                medi.MediType = mediViewModel.MediType;
                medi.MediManager = await _userManager.FindByEmailAsync(mediViewModel.MediManagerEmail ?? string.Empty);
                medi.MediDescription = mediViewModel.MediDescription;

                if(MediBackground != null)
                {

                    using var stream = new MemoryStream();
                    await MediBackground.CopyToAsync(stream);
                    medi.MediBackground = stream.ToArray();
                }
                _dbContext.Medicines.Update(medi);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("ManageMedi");
            }

            return NotFound();
        }


        public async Task<IActionResult> DeleteMedi(int mediId)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("ManageMedi");
            }
            var medi = await _dbContext.Medicines.FindAsync(mediId);
            if (medi != null)
            {
                _dbContext.Remove(medi);
                await _dbContext.SaveChangesAsync();

                return Redirect("ManageMedi");
            }
            return NotFound();

        }
    }
}