using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlib.Utilities
{
    /// <summary>
    /// Specific DateTime functions.
    /// </summary>
    public static class DateTimeEx
    {
        /// <summary>
        /// The <code>DateTime</code> of the unix epoch.
        /// </summary>
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Using unix epoch, as it's a nice base.
        /// </summary>
        private static readonly ulong SecondInTicks = (ulong)(new DateTime(1970, 1, 1, 0, 0, 1, DateTimeKind.Utc).Ticks - UnixEpoch.Ticks);

        /// <summary>
        /// Returns the UnixTime equivilent of the <code>DateTime</code>.
        /// </summary>
        /// <param name="dt">The <code>DateTime</code> to get the UnixTime from.</param>
        /// <returns></returns>
        public static ulong ToUnixTime(this DateTime dt)
        {
            return (ulong)(dt.Subtract(UnixEpoch)).TotalSeconds;
        }

        /// <summary>
        /// Creates a <code>DateTime</code> from the given unix time.
        /// </summary>
        /// <param name="unixtime">The unix time to create the <code>DateTime</code> from.</param>
        /// <returns></returns>
        public static DateTime FromUnixTime(ulong unixtime)
        {
            return new DateTime(UnixEpoch.Ticks + (long)(unixtime * SecondInTicks));
        }
    }
}
