namespace DiscordBot.DAL.PersistenceLayer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DAL;

    public sealed class DatabasePersistence
    {
        private static DiscordBotDbContext Database { get; set; }

        private bool AutoSaveChanges { get; }

        public static void InitializePersistence(string connectionString)
        {
            Database = DiscordBotDbContextFactory.Create(connectionString);
        }

        public DatabasePersistence(bool autoSaveChanges = true)
        {
            AutoSaveChanges = autoSaveChanges;
        }

        public IQueryable<T> Get<T>() where T : class
        {
            return Database.Set<T>();
        }

        public void AddOrUpdate<T>(T entity) where T : class
        {
            AddOrUpdate(entity, AutoSaveChanges);
        }

        public void AddOrUpdate<T>(T entity, bool autoSaveChanges) where T : class
        {
            Database.AddOrUpdate(entity);

            if (autoSaveChanges)
            {
                SaveChanges();
            }
        }

        public async Task AddOrUpdateAsync<T>(T entity) where T : class
        {
            await AddOrUpdateAsync(entity, AutoSaveChanges);
            await Task.CompletedTask;
        }

        public async Task AddOrUpdateAsync<T>(T entity, bool autoSaveChanges) where T : class
        {
            await Database.AddOrUpdateAsync(entity);

            if (autoSaveChanges)
            {
                await SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        public void AddOrUpdateRange<T>(IEnumerable<T> entities) where T : class
        {
            AddOrUpdateRange(entities, AutoSaveChanges);
        }

        public void AddOrUpdateRange<T>(IEnumerable<T> entities, bool autoSaveChanges) where T : class
        {
            foreach (var entity in entities)
            {
                AddOrUpdate(entity, false);
            }

            if (autoSaveChanges)
            {
                SaveChanges();
            }
        }

        public async Task AddOrUpdateRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            await AddOrUpdateRangeAsync(entities, AutoSaveChanges);
            await Task.CompletedTask;
        }

        public async Task AddOrUpdateRangeAsync<T>(IEnumerable<T> entities, bool autoSaveChanges) where T : class
        {
            foreach (var entity in entities)
            {
                await AddOrUpdateAsync(entity, false);
            }

            if (autoSaveChanges)
            {
                await SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        public void Remove<T>(T entity) where T : class
        {
            Remove(entity, AutoSaveChanges);
        }

        public void Remove<T>(T entity, bool autoSaveChanges) where T : class
        {
            Database.Set<T>().Remove(entity);

            if (autoSaveChanges)
            {
                SaveChanges();
            }
        }

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
        {
            RemoveRange(entities, AutoSaveChanges);
        }

        public void RemoveRange<T>(IEnumerable<T> entities, bool autoSaveChanges) where T : class
        {
            foreach (var entity in entities)
            {
                Remove(entity, false);
            }

            if (autoSaveChanges)
            {
                SaveChanges();
            }
        }

        public void SaveChanges()
        {
            Database.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await Database.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}