using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductManagementSystem.Data;
using ProductManagementSystem.IOptions;
using ProductManagementSystem.IOptions;
using ProductManagementSystem.Models;
using ProductManagementSystem.Repositories;
using ProductManagementSystem.Services;
using ProductManagementSystem.Validation;
using ProductManagementSystem.Configuration;
using Microsoft.Extensions.Logging.Console;


namespace ProductManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Product Manager";

            //configuration
            string env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{env}.json", optional: true) // overrides base
                .Build();

            //DI container
            var services = new ServiceCollection();

            //Logging
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Warning);//supress noise in console application 
            });

            //configuration 
            services.Configure<AppSettings>(config.GetSection(AppSettings.SectionName));

            //Repositories -> Transient if we want changes for every instance, singleton for one
            services.AddSingleton<IProductRepository, ProductRepository>();

            // validation -> transient, stateless
            services.AddTransient<IValidationService , ValidationService>();

            //services Scoped
            services.AddScoped<IProductService, ProductService>();

            //Menu -> Scoped
            services.AddScoped<MenuService>();

            //applogger as singleton demo
            services.AddSingleton<AppLogger>();

            ServiceProvider provider = services.BuildServiceProvider();

            // IProductRepository is Scoped — resolving it from the root provider creates a
            // separate instance from what MenuService gets. Use a single scope so both
            // share the same ProductRepository (and the same in-memory list).
            using IServiceScope scope = provider.CreateScope();
            IServiceProvider sp = scope.ServiceProvider;
            
            // seed data 
            // instead ->var repo = provider.GetService<IProductRepository>();
            var repo = sp.GetService<IProductRepository>();
            //IProductRepository repo = new ProductRepository();
            DataSeeder.Seed(repo);

            //MenuService menu = new(repo);
            //run 
            //var menu = provider.GetRequiredService<MenuService>();
            var menu = sp.GetRequiredService<MenuService>();

            menu.Run();
        }
    }
}