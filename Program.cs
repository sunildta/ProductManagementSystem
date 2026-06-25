using ProductManagementSystem.Data;
using ProductManagementSystem.Display;
using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;

namespace ProductManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Product Manager";

            IProductRepository repo = new ProductRepository();
            DataSeeder.Seed(repo);

            MenuService menu = new(repo);
            menu.Run();
        }
    }
}