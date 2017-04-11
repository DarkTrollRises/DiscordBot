﻿#pragma warning disable 618
namespace DiscordBot.DAL
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DiscordBotDbContext : DbContext
    {
        public DbSet<DiscordUser> DiscordUsers { get; set; }

        public DbSet<DiscordGuild> DiscordGuilds { get; set; }

        public DbSet<DiscordUserDiscordGuild> DiscordUserDiscordGuilds { get; set; }

        public DiscordBotDbContext(DbContextOptions options)
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
            var existingEntity = Set<T>().Local.SingleOrDefault(x => CheckKeys(x, entity)) ??
                                 Set<T>().SingleOrDefault(x => CheckKeys(x, entity));

            if (existingEntity != null)
            {
                Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                Entry(entity).State = EntityState.Added;
            }
        }

        public IEnumerable<object> GetKeyValues<T>(T entity)
        {
            return Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name)
                .ToArray()
                .Select(x => entity.GetType().GetProperty(x).GetValue(entity, null));
        }

        public bool CheckKeys<T>(T e1, T e2)
        {
            var e1Keys = GetKeyValues(e1).ToArray();
            var e2Keys = GetKeyValues(e2).ToArray();

            if (e1Keys.Length == e2Keys.Length)
            {
                return !e1Keys.Where((key1, key2Index) => !key1.Equals(e2Keys[key2Index])).Any();
            }

            return false;
        }
    }
}