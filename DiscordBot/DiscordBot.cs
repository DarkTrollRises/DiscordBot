namespace DiscordBot
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Discord;
    using Discord.WebSocket;

    public class DiscordBot
    {
        public static bool Listen { get; set; }

        public DiscordSocketClient Client { get; set; }

        public async Task StartDiscordBot()
        {
            Client = new DiscordSocketClient();
            if (!Directory.Exists("Log"))
            {
                Directory.CreateDirectory("Log");
            }

            Client.Log += Log;

            await Client.LoginAsync(TokenType.Bot, File.ReadAllText("token.key"));
            await Client.StartAsync();

            Client.Ready += async () =>
            {
                Listen = true;
                await Task.CompletedTask;
            };

            await ListenForConsoleInput();

            await Task.Delay(-1);
        }

        private async Task ListenForConsoleInput()
        {
            while (true)
            {
                if (Listen)
                {
                    var input = Console.ReadLine();

                    switch (input)
                    {
                        case "exit":
                        case "quit":
                            Listen = false;
                            await Client.StopAsync();
                            await Client.LogoutAsync();
                            Environment.Exit(1);
                            break;
                        default:
                            await Log(new LogMessage(LogSeverity.Info, "Console", "Command not recognized"));
                            break;
                    }
                }
            }
        }

        private static Task Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            if (log.Severity == LogSeverity.Critical || log.Severity == LogSeverity.Error ||
                log.Severity == LogSeverity.Warning)
            {
                File.WriteAllText($"Log\\{DateTime.Now:ddMMyyyyHHmmss}.txt", log.ToString());
            }

            return Task.CompletedTask;
        }
    }
}
