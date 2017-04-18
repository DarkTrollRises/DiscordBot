namespace DiscordBot
{
    using System;
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
        private readonly DatabasePersistence persistence = new DatabasePersistence();

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
                    await AddNewUserGuilds(guild);
                }

                foreach (var guild in persistence.Get<DiscordGuild>().Include(x => x.DiscordUserDiscordGuilds).ThenInclude(x => x.DiscordUser).ToList().Where(x => x.Active && Client.Guilds.All(y => y.Id != x.GuildId)))
                {
                    await UserGuildManagement.RemoveGuildAsync(guild);
                }

                foreach (var user in persistence.Get<DiscordUser>().Include(x => x.DiscordUserDiscordGuilds).ThenInclude(x => x.DiscordGuild).ToList().Where(x => x.Active && Client.Guilds.SelectMany(y => y.Users).Distinct().All(y => y.Id != x.UserId)))
                {
                    await UserGuildManagement.RemoveUserAsync(user);
                }

                await LogHandler.Log(new LogMessage(LogSeverity.Info, "EntryUpdate",
                    "Succeeded in updating of users and guilds"));

                Client.UserUpdated += async (oldUser, newUser) =>
                {
                    if (!newUser.IsBot && oldUser.Username != newUser.Username)
                    {
                        await UserGuildManagement.AddUserAsync(newUser.ToDiscordUser());
                    }
                };

                Client.UserJoined += async user =>
                {
                    if (!user.IsBot)
                    {
                        await UserGuildManagement.AddOrUpdateUserGuildAsync(user.ToDiscordUser(), user.Guild.ToDiscordGuild());
                    }
                };

                Client.UserUnbanned += async (user, guild) =>
                {
                    if (!user.IsBot)
                    {
                        await UserGuildManagement.AddOrUpdateUserGuildAsync(user.ToDiscordUser(), guild.ToDiscordGuild());
                    }
                };

                Client.UserLeft += async user =>
                {
                    if (!user.IsBot)
                    {
                        await UserGuildManagement.RemoveUserGuildAsync(user.ToDiscordUser(), user.Guild.ToDiscordGuild());
                    }
                };

                Client.UserBanned += async (user, guild) =>
                {
                    if (!user.IsBot)
                    {
                        await UserGuildManagement.RemoveUserGuildAsync(user.ToDiscordUser(), guild.ToDiscordGuild());
                    }
                };

                Client.GuildUpdated += async (oldGuild, newGuild) =>
                {
                    if (oldGuild.Name != newGuild.Name)
                    {
                        await UserGuildManagement.AddOrUpdateGuildAsync(newGuild.ToDiscordGuild());
                    }
                };

                Client.GuildMemberUpdated += async (oldUser, newUser) =>
                {
                    if (oldUser.Nickname != newUser.Nickname)
                    {
                        await UserGuildManagement.AddOrUpdateUserGuildAsync(newUser.ToDiscordUser(),
                            newUser.Guild.ToDiscordGuild(), newUser.Nickname);
                    }
                };

                Client.JoinedGuild += async guild => await AddNewUserGuilds(guild);
                Client.LeftGuild += async guild => await UserGuildManagement.RemoveGuildAsync(guild.ToDiscordGuild());
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

        private static async Task AddNewUserGuilds(SocketGuild guild)
        {
            if (guild.Users.Count(x => !x.IsBot) > 0)
            {
                foreach (var user in guild.Users.Where(x => !x.IsBot))
                {
                    var newUserGuild = guild.Users.First(x => x.Id == user.Id).ToDiscordUserDiscordGuild();
                    newUserGuild.Active = true;

                    await UserGuildManagement.AddOrUpdateUserGuildAsync(newUserGuild);
                }
            }
            else
            {
                await UserGuildManagement.AddOrUpdateGuildAsync(guild.ToDiscordGuild());
            }
        }
    }
}
