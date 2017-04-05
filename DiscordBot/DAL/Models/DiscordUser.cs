namespace DiscordBot.DAL.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DiscordUser
    {
        [Key]
        public long Id { get; set; }

        [NotMapped]
        public ulong UserId {
            get { return (ulong) Id + long.MaxValue; }
            set { Id = (long) (value - long.MaxValue); }
        }
    }
}