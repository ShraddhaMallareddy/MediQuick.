using MediQuickFinal.AppDbContext;
using MediQuickFinal.Models;
using MediQuickFinal.Models.MembershipViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediQuickFinal.Controllers
{
    [Authorize]
    public class MembershipController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public MembershipController(AppIdentityDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var memberships = new List<Members>
            {
                new Members { Id = 1, Name = "Plus Membership", Description = "Delivery within 2 days", Price = 999 },
                new Members { Id = 2, Name = "Premium Membership", Description = "Delivery within 24 hours and extra 5% discount", Price = 1500 }
            };

            return View(memberships);
        }

        [HttpPost]
        public async Task<IActionResult>Buy(int Id, decimal price)
        {
            var members = await _context.Members.FindAsync(Id);
            if (members == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(m => m.ItemId == Id && m.ItemType == "Membership");
            if (cartItem != null)
            {
                cartItem.Quantity += 1;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    ItemId = members.Id,
                    Name = members.Name,
                    Price = price,
                    Quantity = 1,
                    ItemType = "Membership"
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
