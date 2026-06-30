using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductManagementSystem.Models
{
    /// <summary>
    /// Business rule validation (required, length, range) now lives in
    /// Validators/ProductValidator.cs using FluentValidation — not here.
    /// Only EF Core schema/column attributes remain on this model.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Unit Price")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // ── Foreign Keys ──────────────────────────────────────────────────────────
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }

        // ── Navigation Properties ─────────────────────────────────────────────────
        public Category Category { get; set; } = null!;
        public Supplier Supplier { get; set; } = null!;
        public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();

        public override string ToString() =>
            $"[{Id:D3}] {Name,-25} | {Category?.Name ?? "?",-15} | ${Price,8:F2} | Stock: {Stock,4}";
    }
}