using MediQuickFinal.Models;
using MediQuickFinal.Models.MembershipViewModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MediQuickFinal.AppDbContext
{
    public class AppIdentityDbContext : IdentityDbContext<MediUser>
    {
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Members> Members { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {
        }
    }
}

