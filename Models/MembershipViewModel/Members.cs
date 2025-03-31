using System.ComponentModel.DataAnnotations;

namespace MediQuickFinal.Models.MembershipViewModel
{
    public class Members
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
