using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.Models
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        //Foreign key
        public int ProductId { get; set; }

        //Navigation Many review -> one product 
        public Product Product { get; set; } = null!;

        public override string ToString() =>
            $"[{Id:D3}] ★{Rating}/5 — {Comment}";
    }
}
