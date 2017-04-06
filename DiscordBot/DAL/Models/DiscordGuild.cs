#pragma warning disable 618
namespace DiscordBot.DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Utilities;

    public class DiscordGuild
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Obsolete("Use DiscordGuild.GuildId instead")]
        public long Id { get; set; }

        [Required]
        [StringLength(64)]
        public string GuildName { get; set; }

        [NotMapped]
        public ulong GuildId {
            get => Id.ConvertToUlong();
            set => Id = value.ConvertToLong();
        }

        [Required]
        public bool Active { get; set; }

        public virtual List<DiscordUserDiscordGuild> DiscordUserDiscordGuilds { get; set; }
    }
}