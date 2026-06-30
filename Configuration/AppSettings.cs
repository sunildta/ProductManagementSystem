using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Strongly-typed binding for the "AppSettings" section in appsettings.json.
/// Injected via IOptions&lt;AppSettings&gt;.
/// </summary>

namespace ProductManagementSystem.Configuration
{
    public class AppSettings
    {
        public const string SectionName = "AppSettings";
        //tax rate 
        public decimal TaxRate { get; set; } = 0.18m;
        //discount percentage
        public decimal DiscountPercentage { get; set; } = 10m;
        //threshold alert 
        public int MinimumStockLevel { get; set; } = 5;
        //max allowed character in product name 
        public int MaxProductNameLength { get; set; } = 200;

    }
}
