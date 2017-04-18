#pragma warning disable 618
namespace DiscordBot.DAL.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Utilities;

    public class DiscordUserDiscordGuild
    {
        [Obsolete("Use DiscordUserDiscordGuild.UserId instead")]
        public long DiscordUser_Id { get; set; }

        [ForeignKey("DiscordUser_Id")]
        public virtual DiscordUser DiscordUser { get; set; }

        [NotMapped]
        public ulong UserId
        {
            get => DiscordUser_Id.ConvertToUlong();
            set => DiscordUser_Id = value.ConvertToLong();
        }

        [Obsolete("Use DiscordUserDiscordGuild.GuildId instead")]
        public long DiscordGuild_Id { get; set; }

        [ForeignKey("DiscordGuild_Id")]
        public virtual DiscordGuild DiscordGuild { get; set; }

        [NotMapped]
        public ulong GuildId
        {
            get => DiscordGuild_Id.ConvertToUlong();
            set => DiscordGuild_Id = value.ConvertToLong();
        }

        [StringLength(32)]
        public string Nickname { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}