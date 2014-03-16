using System;

namespace MinaGlosor.Web.Infrastructure
{
    public static class SystemTime
    {
        public static Func<DateTime> UtcDateTime;

        public static DateTime UtcNow
        {
            get
            {
                var func = UtcDateTime;
                return func != null ? func() : DateTime.UtcNow;
            }
        }
    }
}