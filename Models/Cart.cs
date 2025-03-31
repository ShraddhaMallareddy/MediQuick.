using System.Collections.Generic;
using System.Linq;

namespace MediQuickFinal.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public List<CartItem> Medicines { get; set; } = new List<CartItem>();
        public List<CartItem> Memberships { get; set; } = new List<CartItem>();
        public decimal TotalPrice => Medicines.Sum(m => m.Price * m.Quantity) + Memberships.Sum(m => m.Price * m.Quantity);
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; } 
        public string ItemType { get; set; } 
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
       
    }
}
