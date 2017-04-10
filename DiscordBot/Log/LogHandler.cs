namespace DiscordBot.Log
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Discord;

    public static class LogHandler
    {
        public static Task Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            if (log.Severity == LogSeverity.Critical || log.Severity == LogSeverity.Error ||
                log.Severity == LogSeverity.Warning)
            {
                File.WriteAllText($"Log\\{DateTime.Now:yyMMddHHmmss}.txt", log.ToString());
            }

            return Task.CompletedTask;
        }
    }
}