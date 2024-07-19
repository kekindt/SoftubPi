using Microsoft.EntityFrameworkCore;
namespace softubWeb.Models;
public partial class ConfigContext : DbContext
{
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           var configDbPath = Environment.GetEnvironmentVariable("CONFIG_DB");
           if(string.IsNullOrEmpty(configDbPath))
                configDbPath = "c:\\projects\\suftubConfig\\config.db";
           //optionsBuilder.UseSqlite("Data Source=c:\\projects\\softubConfig\\config.db");
           optionsBuilder.UseSqlite($"Data Source={configDbPath}");
        }

        
}