namespace DiscordBot.Utilities
{
    using DAL.Models;
    using Discord.WebSocket;

    public static class SocketGuildUserExtension
    {
        public static DiscordUserDiscordGuild ToDiscordUserDiscordGuild(this SocketGuildUser guildUser)
        {
            return new DiscordUserDiscordGuild { DiscordGuild = guildUser.Guild.ToDiscordGuild(), GuildId = guildUser.Guild.Id, DiscordUser = guildUser.ToDiscordUser(), UserId = guildUser.Id, Nickname = guildUser.Nickname };
        }
    }
}