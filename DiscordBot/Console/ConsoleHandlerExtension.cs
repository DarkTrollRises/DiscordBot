namespace DiscordBot.Console
{
    using System;
    using System.Threading.Tasks;
    using Discord;
    using Discord.WebSocket;
    using Log;

    public static class ConsoleHandlerExtension
    {
        public static bool Listen { get; set; }

        public static async Task HandleConsoleInput(this DiscordSocketClient client)
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
                        case "stop":
                            Listen = false;
                            await client.StopAsync();
                            await client.LogoutAsync();
                            Environment.Exit(0);
                            break;
                        default:
                            await LogHandler.Log(new LogMessage(LogSeverity.Info, "Console", "Command not recognized"));
                            break;
                    }
                }
            }
        }
    }
}