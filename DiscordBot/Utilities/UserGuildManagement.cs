namespace DiscordBot.Utilities
{
    using System.Linq;
    using System.Threading.Tasks;
    using DAL.Models;
    using DAL.PersistenceLayer;

    public static class UserGuildManagement
    {
        private static DatabasePersistence persistence = new DatabasePersistence(false);

        public static async Task AddUser(DiscordUser user, bool autoSaveChanges = true)
        {
            user.Active = true;
            persistence.AddOrUpdate(user);

            if (autoSaveChanges)
            {
                persistence.SaveChanges();
            }

            await Task.CompletedTask;
        }

        public static async Task AddGuild(DiscordGuild guild, bool autoSaveChanges = true)
        {
            guild.Active = true;
            persistence.AddOrUpdate(guild);

            if (autoSaveChanges)
            {
                persistence.SaveChanges();
            }

            await Task.CompletedTask;
        }

        public static async Task AddUserGuild(DiscordUser user, DiscordGuild guild)
        {
            await AddUser(user, false);
            await AddGuild(guild, false);
            persistence.AddOrUpdate(new DiscordUserDiscordGuild { UserId = user.UserId, GuildId = guild.GuildId, Active = true });
            persistence.SaveChanges();

            await Task.CompletedTask;
        }

        public static async Task RemoveUser(DiscordUser user)
        {
            user.Active = false;
            persistence.AddOrUpdate(user);

            foreach (var userGuild in user.DiscordUserDiscordGuilds)
            {
                userGuild.Active = false;
                persistence.AddOrUpdate(userGuild);
            }

            persistence.SaveChanges();

            await Task.CompletedTask;
        }

        public static async Task RemoveGuild(DiscordGuild guild)
        {
            guild.Active = false;
            persistence.AddOrUpdate(guild);

            foreach (var userGuild in persistence.Get<DiscordUserDiscordGuild>().ToList().Where(x => x.GuildId == guild.GuildId))
            {
                userGuild.Active = false;
                persistence.AddOrUpdate(userGuild);
            }

            persistence.SaveChanges();

            await Task.CompletedTask;
        }

        public static async Task RemoveUserGuild(DiscordUser user, DiscordGuild guild)
        {
            var removedItem = persistence.Get<DiscordUserDiscordGuild>()
                .ToList()
                .FirstOrDefault(x => x.UserId == user.UserId && x.GuildId == guild.GuildId);

            if (removedItem != null)
            {
                removedItem.Active = false;
                persistence.AddOrUpdate(removedItem, true);
            }

            await Task.CompletedTask;
        }

        public static void RenewPersistence()
        {
            DatabasePersistence.RenewDatabase();
        }
    }
}