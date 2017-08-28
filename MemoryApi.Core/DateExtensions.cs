using System;

namespace MemoryServer.Core
{
    public static class DateExtensions
    {
        public static DateTime Floor(this DateTime dateTime, TimeSpan interval)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % interval.Ticks));
        }

        public static DateTime Ceil(this DateTime dateTime, TimeSpan interval)
        {
            var overflow = dateTime.Ticks % interval.Ticks;

            return overflow == 0 ? dateTime : dateTime.AddTicks(interval.Ticks - overflow);
        }

        public static DateTime Round(this DateTime dateTime, TimeSpan interval)
        {
            var halfIntervelTicks = (interval.Ticks + 1) >> 1;

            return dateTime.AddTicks(halfIntervelTicks - ((dateTime.Ticks + halfIntervelTicks) % interval.Ticks));
        }

        public static long ToUnixMillis(this DateTime @this) => (long) (@this - new DateTime(1970, 1, 1)).TotalMilliseconds;
    }
}