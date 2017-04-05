namespace DiscordBot.DAL
{
    using Microsoft.EntityFrameworkCore;
    using MySQL.Data.EntityFrameworkCore.Extensions;

    /// <summary>
    /// Factory class for DiscordBotDbContext
    /// </summary>
    public static class DiscordBotDbContextFactory
    {
        public static DiscordBotDbContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DiscordBotDbContext>();
            optionsBuilder.UseMySQL(connectionString);

            //Ensure database creation
            var context = new DiscordBotDbContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}