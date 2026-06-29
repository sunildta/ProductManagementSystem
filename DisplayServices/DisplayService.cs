using ProductManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.IOptions
{
    public static class DisplayService
    {
        private const int Width = 70;
        private static readonly string Divider = new('-', Width);

        public static void Header(string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Divider);
            Console.WriteLine($"  {title.ToUpperInvariant()}");
            Console.WriteLine(Divider);
            Console.ResetColor();
        }

        public static void PrintProducts(IEnumerable<Product> products, string? emptyMsg = null)
        {
            var list = products.ToList();
            if (list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  {emptyMsg ?? "No products found."}");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  {"[ID]",-6} {"Name",-25} | {"Category",-15} | {"Price",9} | {"Stock",6}");
            Console.WriteLine($"  {new string('-', 64)}");
            Console.ResetColor();

            foreach (var p in list)
            {
                Console.Write("  ");
                Console.WriteLine(p);
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  {list.Count} product(s).");
            Console.ResetColor();
        }

        public static void PrintValue(string label, decimal value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  {label}: ${value:N2}");
            Console.ResetColor();
        }

        public static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  ✔ {msg}");
            Console.ResetColor();
        }

        public static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  ✘ {msg}");
            Console.ResetColor();
        }
    }
}
