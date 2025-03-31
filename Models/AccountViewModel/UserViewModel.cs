using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediQuickFinal.Models.AccountViewModel
{
    public class UserViewModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public MediRole Role { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; } = [];
    }
}
