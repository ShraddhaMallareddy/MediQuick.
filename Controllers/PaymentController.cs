using Microsoft.AspNetCore.Mvc;

namespace MediQuickFinal.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessPayment(string cardNumber, string expiryDate, string cvv)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
