using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediQuickFinal.Models.AccountViewModel
{
    public class CreateUserViewModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? ConfirmEmail { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public MediRole Role { get; set; }

        public List<SelectListItem> Roles
        {
            get
            {
                return Enum.GetValues<MediRole>()
                    .Select(x => new SelectListItem
                    {
                        Text = Enum.GetName(x),
                        Value = x.ToString()
                    }).ToList();

            }
        }
    }
}