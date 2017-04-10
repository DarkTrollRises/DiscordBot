namespace DiscordBot.Utilities
{
    using DAL.Models;
    using Discord.WebSocket;

    public static class SocketGuildExtension
    {
        public static DiscordGuild ToDiscordGuild(this SocketGuild guild)
        {
            return new DiscordGuild { GuildId = guild.Id, GuildName = guild.Name };
        }
    }
}