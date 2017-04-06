using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DiscordBot.DAL;

namespace DiscordBot.Migrations
{
    [DbContext(typeof(DiscordBotDbContext))]
    partial class DiscordBotDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("DiscordBot.DAL.Models.DiscordGuild", b =>
                {
                    b.Property<long>("Id");

                    b.Property<bool>("Active");

                    b.Property<string>("GuildName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("DiscordGuilds");
                });

            modelBuilder.Entity("DiscordBot.DAL.Models.DiscordUser", b =>
                {
                    b.Property<long>("Id");

                    b.Property<bool>("Active");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("DiscordUsers");
                });

            modelBuilder.Entity("DiscordBot.DAL.Models.DiscordUserDiscordGuild", b =>
                {
                    b.Property<long>("DiscordUser_Id");

                    b.Property<long>("DiscordGuild_Id");

                    b.Property<bool>("Active");

                    b.HasKey("DiscordUser_Id", "DiscordGuild_Id");

                    b.HasIndex("DiscordGuild_Id");

                    b.ToTable("DiscordUserDiscordGuilds");
                });

            modelBuilder.Entity("DiscordBot.DAL.Models.DiscordUserDiscordGuild", b =>
                {
                    b.HasOne("DiscordBot.DAL.Models.DiscordGuild", "DiscordGuild")
                        .WithMany("DiscordUserDiscordGuilds")
                        .HasForeignKey("DiscordGuild_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DiscordBot.DAL.Models.DiscordUser", "DiscordUser")
                        .WithMany("DiscordUserDiscordGuilds")
                        .HasForeignKey("DiscordUser_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
