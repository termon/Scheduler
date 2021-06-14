using System;

namespace Scheduler.Core.Extensions
{
    public static class DateTimeExtensions
    {
        // Calculate start of week from datetime
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}