﻿#pragma warning disable 618
namespace DiscordBot.DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Utilities;

    public class DiscordUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Obsolete("Use DiscordUser.UserId instead")]
        public long Id { get; set; }

        [Required]
        [StringLength(32)]
        public string Username { get; set; }

        [NotMapped]
        public ulong UserId
        {
            get => Id.ConvertToUlong();
            set => Id = value.ConvertToLong();
        }

        [Required]
        public bool Active { get; set; }

        public virtual List<DiscordUserDiscordGuild> DiscordUserDiscordGuilds { get; set; }
    }
}