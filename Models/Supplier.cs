using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        //Navigation 
        //One Supplier Many Product
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public override string ToString() =>
            $"[{Id:D3}] {Name,-20} | {Email,-25} | {Phone}";
    }
}
