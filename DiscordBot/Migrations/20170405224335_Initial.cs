using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordBot.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscordGuilds",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    GuildName = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordGuilds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Username = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordUserDiscordGuilds",
                columns: table => new
                {
                    DiscordUser_Id = table.Column<long>(nullable: false),
                    DiscordGuild_Id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordUserDiscordGuilds", x => new { x.DiscordUser_Id, x.DiscordGuild_Id });
                    table.ForeignKey(
                        name: "FK_DiscordUserDiscordGuilds_DiscordGuilds_DiscordGuild_Id",
                        column: x => x.DiscordGuild_Id,
                        principalTable: "DiscordGuilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscordUserDiscordGuilds_DiscordUsers_DiscordUser_Id",
                        column: x => x.DiscordUser_Id,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordUserDiscordGuilds_DiscordGuild_Id",
                table: "DiscordUserDiscordGuilds",
                column: "DiscordGuild_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordUserDiscordGuilds");

            migrationBuilder.DropTable(
                name: "DiscordGuilds");

            migrationBuilder.DropTable(
                name: "DiscordUsers");
        }
    }
}
