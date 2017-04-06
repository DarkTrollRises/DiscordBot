namespace RunDiscordBot
{
    using DiscordBot;

    public class Run
    {
        public static void Main(string[] args) => new DiscordBot().StartDiscordBot().GetAwaiter().GetResult();
    }
}
