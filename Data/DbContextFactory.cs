using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProductManagementSystem.Data
{
    /// <summary>
    /// Allows the EF Core CLI tools (dotnet ef migrations add, dotnet ef database update)
    /// to create an ApplicationDbContext at design time without running Program.Main().
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Build configuration the same way Program.cs does
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();


            string connectionString = config["Database:ConnectionString"]
                ?? "Host=localhost;Port=port;Database=database;Username=username;Password=password";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
