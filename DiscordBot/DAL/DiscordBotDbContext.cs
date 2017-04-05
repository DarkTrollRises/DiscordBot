namespace DiscordBot.DAL
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DiscordBotDbContext : DbContext
    {
        public DbSet<DiscordUser> DiscordUsers { get; set; }

        public DiscordBotDbContext(DbContextOptions<DiscordBotDbContext> options)
            : base(options)
        {
        }

        public void AddOrUpdate<T>(T entity) where T : class
        {
            var existingEntity = Set<T>().Find(entity);

            SetEntityState(entity, existingEntity);
        }

        public async Task AddOrUpdateAsync<T>(T entity) where T : class
        {
            var existingEntity = await Set<T>().FindAsync(entity);

            SetEntityState(entity, existingEntity);
            await Task.CompletedTask;
        }

        private void SetEntityState<T>(T entity, T existingEntity) where T : class
        {
            if (existingEntity != null)
            {
                Entry(existingEntity).State = EntityState.Modified;
            }
            else
            {
                Entry(entity).State = EntityState.Added;
            }
        }
    }
}