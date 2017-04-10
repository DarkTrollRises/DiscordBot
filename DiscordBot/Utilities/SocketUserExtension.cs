namespace DiscordBot.Utilities
{
    using DAL.Models;
    using Discord.WebSocket;

    public static class SocketUserExtension
    {
        public static DiscordUser ToDiscordUser(this SocketUser user)
        {
            return new DiscordUser { UserId = user.Id, Username = user.Username };
        }
    }
}