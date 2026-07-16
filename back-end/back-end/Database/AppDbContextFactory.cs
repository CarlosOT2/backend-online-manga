using back_end.Shared.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace back_end.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Terminal")
                ?? throw new InvalidOperationException("Connection string 'Terminal' not found.");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            var validationSettings = configuration
                .GetSection("Database:Tables:Validation")
                .Get<ValidationSettings>()
                ?? throw new InvalidOperationException("Seção 'Database:Tables:Validation' não encontrada no appsettings.json.");

            return new AppDbContext(optionsBuilder.Options, Options.Create(validationSettings!));
        }
    }
}