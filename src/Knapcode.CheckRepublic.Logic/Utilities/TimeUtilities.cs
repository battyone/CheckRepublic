using System;

namespace Knapcode.CheckRepublic.Logic.Utilities
{
    public static class TimeUtilities
    {
        public static long DateTimeOffsetToLong(DateTimeOffset input)
        {
            return input.UtcTicks;
        }

        public static long TimeSpanToLong(TimeSpan input)
        {
            return input.Ticks;
        }

        public static DateTimeOffset LongToDateTimeOffset(long input)
        {
            return new DateTimeOffset(input, TimeSpan.Zero);
        }

        public static TimeSpan LongToTimeSpan(long input)
        {
            return new TimeSpan(input);
        }
    }
}
