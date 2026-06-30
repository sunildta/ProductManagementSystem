using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProductManagementSystem.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // ── Navigation ────────────────────────────────────────────────────────────
        //one category many product
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public override string ToString() =>
            $"[{Id:D3}] {Name,-20} | {Description}";
 
        
    }
}
