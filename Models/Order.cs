namespace MediQuickFinal.Models
{
    public class Order
    {
            public int Id { get; set; }
            public string UserId { get; set; }
            public DateTime OrderDate { get; set; }
            public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
            public decimal TotalPrice { get; set; }
        }

        public class OrderItem
        {
            public int Id { get; set; }
            public int OrderId { get; set; }
            public int ItemId { get; set; }
            public string ItemType { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }

    }