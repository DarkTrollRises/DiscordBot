namespace DiscordBot
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Console;
    using DAL.Models;
    using DAL.PersistenceLayer;
    using Discord;
    using Discord.WebSocket;
    using Log;
    using Microsoft.EntityFrameworkCore;
    using Utilities;

    public class DiscordBot
    {
        private DatabasePersistence persistence = new DatabasePersistence();

        public DiscordSocketClient Client { get; set; }

        public async Task StartDiscordBot()
        {
            Client = new DiscordSocketClient();
            if (!Directory.Exists("Log"))
            {
                Directory.CreateDirectory("Log");
            }

            Client.Log += LogHandler.Log;

            await Client.LoginAsync(TokenType.Bot, File.ReadAllText("token.key"));
            await Client.StartAsync();

            Client.Ready += async () =>
            {
                ConsoleHandlerExtension.Listen = true;
                await InitUserGuilds();
                await Task.CompletedTask;
            };

            await Client.HandleConsoleInput();

            await Task.Delay(-1);
        }

        private async Task InitUserGuilds()
        {
            await LogHandler.Log(new LogMessage(LogSeverity.Info, "EntryUpdate",
                "Started updating users and guilds"));

            try
            {
                foreach (var guild in Client.Guilds)
                {
                    var newGuild = new DiscordGuild { GuildId = guild.Id, GuildName = guild.Name };

                    if (guild.Users.Count(x => !x.IsBot) > 0)
                    {
                        foreach (var user in guild.Users.Where(x => !x.IsBot))
                        {
                            var newUser = new DiscordUser { UserId = user.Id, Username = user.Username };
                            await UserGuildManagement.AddUserGuild(newUser, newGuild);
                        }
                    }
                    else
                    {
                        await UserGuildManagement.AddGuild(newGuild);
                    }

                    UserGuildManagement.RenewPersistence();
                }

                foreach (var guild in persistence.Get<DiscordGuild>().Include(x => x.DiscordUserDiscordGuilds).ToList().Where(x => x.Active && Client.Guilds.All(y => y.Id != x.GuildId)))
                {
                    await UserGuildManagement.RemoveGuild(guild);
                }

                foreach (var user in persistence.Get<DiscordUser>().Include(x => x.DiscordUserDiscordGuilds).ToList().Where(x => x.Active && Client.Guilds.SelectMany(y => y.Users).Distinct().All(y => y.Id != x.UserId)))
                {
                    await UserGuildManagement.RemoveUser(user);
                }

                await LogHandler.Log(new LogMessage(LogSeverity.Info, "EntryUpdate",
                    "Succeeded in updating of users and guilds"));
            }
            catch (Exception e)
            {
                await LogHandler.Log(new LogMessage(LogSeverity.Error, "EntryUpdate", "An error has occurred", e));
            }
            finally
            {
                await Task.CompletedTask;
            }
        }
    }
}
