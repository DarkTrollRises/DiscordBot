namespace DiscordBot.Utilities
{
    public static class LongExtension
    {
        public static ulong ConvertToUlong(this long source)
        {
            return (ulong) (source - long.MinValue);
        }

        public static long ConvertToLong(this ulong source)
        {
            return (long) source + long.MinValue;
        }
    }
}