using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BoardGamesBot.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Шлях до `appsettings.json` в іншому проекті
            var basePath = Path.Combine(Directory.GetCurrentDirectory());

            // Завантаження конфігурації
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(configuration.GetConnectionString("SqlConnectionString"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("SqlConnectionString")));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}