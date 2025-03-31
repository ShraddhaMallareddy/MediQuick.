using MediQuickFinal.AppDbContext;
using MediQuickFinal.Models;
using MediQuickFinal.Models.AdminViewModel;
using MediQuickFinal.Models.MedicinesViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MediQuickFinal.Controllers
{
    
public class MedicinesController : Controller
    {
        private readonly UserManager<MediUser> _userManager;
        private readonly AppIdentityDbContext _dbContext;

        public MedicinesController(AppIdentityDbContext dbContext, UserManager<MediUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        //[Authorize(Policy = "MustbeAGuest")]
        public async Task<IActionResult> Index(MedicineViewModel medi, string filterType, string filterCategory)
        {
            var medicines = from m in _dbContext.Medicines select m;

            if (!String.IsNullOrEmpty(filterType))
            {
                medicines = medicines.Where(m => m.MediType.ToString() == filterType);
            }

            if (!String.IsNullOrEmpty(filterCategory))
            {
                if (Enum.TryParse(filterCategory, out MediCategory category))
                {
                    medicines = medicines.Where(m => m.MediCategory == category);
                }
            }

            var listOfMedi = await medicines.ToListAsync();

            var listOfMediViewModel = listOfMedi.Select(medi => new MedicineViewModel
            {
                MediId = medi.MediId,
                MediName = medi.MediName,
                MediPrice = medi.MediPrice,
                MediDescription = medi.MediDescription,
                MediCategory = medi.MediCategory,
                MediType = medi.MediType,
                MediManager = medi.MediManager,
                MediBackground = medi.MediBackground
            }).ToList();

            return View(listOfMediViewModel);
        }
        //[Authorize(Policy = "MustbeAGuest")]
        public async Task<IActionResult> MediDetails(int MediId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }



            var mediDetails = await _dbContext.Medicines.Include(x => x.MediManager).FirstOrDefaultAsync(y => y.MediId == MediId);
            if (mediDetails == null)
            {
                return RedirectToAction("Index");
            }
            var mediDetailsViewModel = new MedicineViewModel
            {
                MediName = mediDetails.MediName,
                MediDescription = mediDetails.MediDescription,
                MediType = mediDetails.MediType,
                MediManagerEmail = mediDetails.MediManager?.Email,
                MediLogo = mediDetails.MediLogo,
                MediBanner =mediDetails.MediBanner,
                MediBackground = mediDetails.MediBackground,
            };
            return View(mediDetailsViewModel);
        }
        //[Authorize(Policy = "MustbeAGuest")]
        public IActionResult CreateMediForCustomer()
        {

            return View(new MedicineViewModel());
        }
        //[Authorize(Policy = "MustbeAGuest")]
        [HttpPost]
        public async Task<IActionResult> CreateMediForCustomer(MedicineViewModel medicineViewModel, IFormFile medilogo, IFormFile mediBanner, IFormFile mediBackground)
        {
            if (!ModelState.IsValid)
            {
                return View(medicineViewModel);
            }

            var loggedInUserEmail = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (loggedInUserEmail == null) { return RedirectToAction("Login", "Account", new { returnUrl = "/Medicines/CreateMediForCustomer" }); }
            var loggedInUser = await _userManager.FindByEmailAsync(loggedInUserEmail);
            if (medicineViewModel != null && loggedInUser != null)
            {


                var claims = new List<Claim>
            {
                new(ClaimTypes.Role,"MediManager"),
                new("MediId",medicineViewModel.MediId.ToString()),
                new("MediName",medicineViewModel.MediName ?? ""),
                new(ClaimTypes.Role,"Guest") };



                await _userManager.AddClaimsAsync(loggedInUser, claims);


                await _userManager.UpdateAsync(loggedInUser);
                Medicine medi = new()
                {
                    MediName = medicineViewModel.MediName,
                    MediDescription = medicineViewModel.MediDescription,
                    MediCategory = medicineViewModel.MediCategory,
                    MediType = medicineViewModel.MediType,
                    MediManager = loggedInUser,
                   
                };
                if (medilogo != null)
                {
                    using var memoryStream = new MemoryStream();
                    await medilogo.CopyToAsync(memoryStream);
                    medi.MediLogo = memoryStream.ToArray();


                }
                if (mediBanner != null)
                {
                    using var memoryStream = new MemoryStream();
                    await mediBanner.CopyToAsync(memoryStream);
                    medi.MediBanner = memoryStream.ToArray();


                }
                if (mediBackground != null)
                {
                    using var memoryStream = new MemoryStream();
                    await mediBackground.CopyToAsync(memoryStream);
                    medi.MediBackground = memoryStream.ToArray();


                }
                _dbContext.Medicines.Add(medi);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View("Index", "Medicine");


        }

    }
}
