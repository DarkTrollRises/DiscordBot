namespace DiscordBot.DAL.PersistenceLayer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using DAL;

    public sealed class DatabasePersistence
    {
        private static DiscordBotDbContext Database { get; } = GetContext();

        private static string DatabaseSettings { get; set; }

        private bool AutoSaveChanges { get; }

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

        public void Dispose()
        {
            Database.Dispose();
        }

        private static DiscordBotDbContext GetContext()
        {
            if (string.IsNullOrEmpty(DatabaseSettings))
            {
                DatabaseSettings = Path.Combine(AppContext.BaseDirectory,
                    string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar));
            }

            return new DiscordBotDbContextFactory().Create(DatabaseSettings);
        }
    }
}