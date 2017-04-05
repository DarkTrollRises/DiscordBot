namespace RunDiscordBot
{
    using System;
    using DiscordBot;
    using DiscordBot.DAL.PersistenceLayer;
    using Microsoft.Extensions.Configuration;

    public class Run
    {
        public static void Main(string[] args)
        {
            var connectionString = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .Build()
                .GetConnectionString("DiscordBotDbContext");

            if (!string.IsNullOrEmpty(connectionString))
            {
                DatabasePersistence.InitializePersistence(connectionString);

                new DiscordBot().StartDiscordBot().GetAwaiter().GetResult();
            }
        }
    }
}
