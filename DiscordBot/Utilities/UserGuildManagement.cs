namespace DiscordBot.Utilities
{
    using System.Linq;
    using System.Threading.Tasks;
    using DAL.Models;
    using DAL.PersistenceLayer;

    public static class UserGuildManagement
    {
        private static readonly DatabasePersistence persistence = new DatabasePersistence(false);

        public static async Task AddUserAsync(DiscordUser user, bool autoSaveChanges = true)
        {
            await Task.Run(() => AddUser(user, autoSaveChanges));
        }

        public static void AddUser(DiscordUser user, bool autoSaveChanges = true)
        {
            user.Active = true;
            persistence.AddOrUpdate(user);

            if (autoSaveChanges)
            {
                persistence.SaveChanges();
            }
        }

        public static async Task AddGuildAsync(DiscordGuild guild, bool autoSaveChanges = true)
        {
            await Task.Run(() => AddGuild(guild, autoSaveChanges));
        }

        public static void AddGuild(DiscordGuild guild, bool autoSaveChanges = true)
        {
            guild.Active = true;
            persistence.AddOrUpdate(guild);

            if (autoSaveChanges)
            {
                persistence.SaveChanges();
            }
        }

        public static async Task AddUserGuildAsync(DiscordUser user, DiscordGuild guild, bool autoSaveChanges = true)
        {
            await Task.Run(() => AddUserGuild(user, guild, autoSaveChanges));
        }

        public static void AddUserGuild(DiscordUser user, DiscordGuild guild, bool autoSaveChanges = true)
        {
            AddUser(user, false);
            AddGuild(guild, false);
            persistence.AddOrUpdate(new DiscordUserDiscordGuild { UserId = user.UserId, GuildId = guild.GuildId, Active = true });

            if (autoSaveChanges)
            {
                persistence.SaveChanges();
            }
        }

        public static async Task RemoveUserAsync(DiscordUser user, bool autoSaveChanges = true)
        {
            await Task.Run(() => RemoveUser(user, autoSaveChanges));
        }

        public static void RemoveUser(DiscordUser user, bool autoSaveChanges = true)
        {
            user.Active = false;
            persistence.AddOrUpdate(user);

            foreach (var userGuild in user.DiscordUserDiscordGuilds)
            {
                RemoveUserGuild(userGuild, false);
            }

            if (autoSaveChanges)
            {
                persistence.SaveChanges();
            }
        }

        public static async Task RemoveGuildAsync(DiscordGuild guild, bool autoSaveChanges = true)
        {
            await Task.Run(() => RemoveGuild(guild, autoSaveChanges));
        }

        public static void RemoveGuild(DiscordGuild guild, bool autoSaveChanges = true)
        {
            guild.Active = false;
            persistence.AddOrUpdate(guild);

            foreach (var userGuild in persistence.Get<DiscordUserDiscordGuild>().ToList().Where(x => x.GuildId == guild.GuildId))
            {
                RemoveUserGuild(userGuild, false);
            }

            if (autoSaveChanges)
            {
                persistence.SaveChanges();
            }
        }

        public static async Task RemoveUserGuildAsync(DiscordUser user, DiscordGuild guild, bool autoSaveChanges = true)
        {
            await Task.Run(() => RemoveUserGuild(user, guild, autoSaveChanges));
        }

        public static void RemoveUserGuild(DiscordUser user, DiscordGuild guild, bool autoSaveChanges = true)
        {
            var removedItem = persistence.Get<DiscordUserDiscordGuild>()
                .ToList()
                .FirstOrDefault(x => x.UserId == user.UserId && x.GuildId == guild.GuildId);

            if (removedItem != null)
            {
                RemoveUserGuild(removedItem, autoSaveChanges);
            }
        }

        public static async Task RemoveUserGuildAsync(DiscordUserDiscordGuild userGuild, bool autoSaveChanges = true)
        {
            await Task.Run(() => RemoveUserGuild(userGuild, autoSaveChanges));
        }

        public static void RemoveUserGuild(DiscordUserDiscordGuild userGuild, bool autoSaveChanges = true)
        {
            userGuild.Active = false;
            persistence.AddOrUpdate(userGuild);

            if (persistence.Get<DiscordUserDiscordGuild>()
                    .Count(x => x.Active && x.UserId == userGuild.UserId && x.GuildId != userGuild.GuildId) == 0)
            {
                var newUser = userGuild.DiscordUser;
                newUser.Active = false;
                persistence.AddOrUpdate(newUser);
            }

            if (persistence.Get<DiscordUserDiscordGuild>()
                    .Count(x => x.Active && x.GuildId == userGuild.GuildId && x.UserId != userGuild.UserId) == 0)
            {
                var newGuild = userGuild.DiscordGuild;
                newGuild.Active = false;
                persistence.AddOrUpdate(newGuild);
            }

            if (autoSaveChanges)
            {
                persistence.SaveChanges();
            }
        }
    }
}