#pragma warning disable 618
namespace DiscordBot.DAL
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DiscordBotDbContext : DbContext
    {
        public DbSet<DiscordUser> DiscordUsers { get; set; }

        public DbSet<DiscordGuild> DiscordGuilds { get; set; }

        public DbSet<DiscordUserDiscordGuild> DiscordUserDiscordGuilds { get; set; }

        public DiscordBotDbContext(DbContextOptions<DiscordBotDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add composite keys here
            modelBuilder.Entity<DiscordUserDiscordGuild>().HasKey(x => new { x.DiscordUser_Id, x.DiscordGuild_Id });
        }

        public void AddOrUpdate<T>(T entity) where T : class
        {
            Entry(entity).State = Set<T>().Attach(entity).State == EntityState.Unchanged
                    ? EntityState.Modified
                    : EntityState.Added;
        }
    }
}