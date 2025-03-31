using Microsoft.AspNetCore.Identity;

namespace MediQuickFinal.Models
{
    public class MediUser : IdentityUser
    {
        public MediRole MediRole { get; set; }
        public string? AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; } = string.Empty;
        public string? AddressLine3 { get; set; } = string.Empty;
        public string? addressLine4 { get; set; } = string.Empty;

    }
    public enum MediRole
    {
        Guest,
        MediManager,
        SuperAdmin
    }
}
