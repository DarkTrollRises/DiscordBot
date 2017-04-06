namespace DiscordBot.DAL
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using MySQL.Data.EntityFrameworkCore.Extensions;

    /// <summary>
    /// Factory class for DiscordBotDbContext
    /// </summary>
    public class DiscordBotDbContextFactory : IDbContextFactory<DiscordBotDbContext>
    {
        public DiscordBotDbContext Create(DbContextFactoryOptions options)
        {
            return Create(options.ContentRootPath);
        }

        public DiscordBotDbContext Create(string contentPath)
        {
            var connectionString = new ConfigurationBuilder()
                .SetBasePath(contentPath)
                .AddJsonFile("appsettings.json", false, true)
                .Build()
                .GetConnectionString("DiscordBotDbContext");

            var settings = new ConfigurationBuilder()
                .SetBasePath(contentPath)
                .AddJsonFile("appsettings.default.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .Build()
                .GetSection("ConnectionSettings");

            connectionString = connectionString
                .Replace("{host}", settings["host"])
                .Replace("{user}", settings["user"])
                .Replace("{pass}", settings["pass"])
                .Replace("{port}", settings["port"])
                .Replace("{database}", settings["database"]);

            return new DiscordBotDbContext(new DbContextOptionsBuilder<DiscordBotDbContext>().UseMySQL(connectionString).Options);
        }
    }
}