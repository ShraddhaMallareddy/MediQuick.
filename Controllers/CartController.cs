using MediQuickFinal.AppDbContext;
using MediQuickFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static MediQuickFinal.Models.Cart;

namespace MediQuickFinal.Controllers
{
    public class CartController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public CartController(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.CartItems.ToListAsync();
            var cart = new Cart
            {
                Medicines = cartItems.Where(ci => ci.ItemType == "Medicine").ToList(),
                Memberships = cartItems.Where(ci => ci.ItemType == "Membership").ToList()
            };
            return View(cart);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine != null)
            {
                var cartItem = await _context.CartItems.FirstOrDefaultAsync(m => m.ItemId == id && m.ItemType == "Medicine");
                if (cartItem != null)
                {
                    cartItem.Quantity += quantity;
                }
                else
                {
                    _context.CartItems.Add(new CartItem
                    {
                        ItemId = medicine.MediId,
                        Name = medicine.MediName,
                        Price = medicine.MediPrice,
                        Quantity = quantity,
                        ItemType = "Medicine"
                    });
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(m => m.ItemId == id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        //public IActionResult Checkout()
        //{
        //    return RedirectToAction("Index", "Payment");
        //}
        public IActionResult Checkout()
        {
            return RedirectToAction("OrderSummary");
        }

        public IActionResult OrderSummary()
        {
            var cartItems = _context.CartItems.ToList();
            var cart = new Cart
            {
                Medicines = cartItems.Where(ci => ci.ItemType == "Medicine").ToList(),
                Memberships = cartItems.Where(ci => ci.ItemType == "Membership").ToList()
            };
            return View(cart);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _context.CartItems.ToListAsync();

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = cartItems.Sum(ci => ci.Price * ci.Quantity)
            };

            foreach (var cartItem in cartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ItemId = cartItem.ItemId,
                    ItemType = cartItem.ItemType,
                    Name = cartItem.Name,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity
                });
            }

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("OrderSuccess");
        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return View(orders);
        }

        //        [HttpPost]
        //        public IActionResult ConfirmOrder()
        //        {

        //            return RedirectToAction("OrderSuccess");
        //        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

    }
}
