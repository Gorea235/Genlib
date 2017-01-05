using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlib.Logging
{
    /// <summary>
    /// A replacement enum for the .NET Framework System.Diagnostics.TraceLevel enum
    /// to enable .NET Core compatibility. Original summary:
    ///   Specifies what messages to output for the <c>System.Diagnostics.Debug</c>, <c>System.Diagnostics.Trace</c>
    ///   and <c>System.Diagnostics.TraceSwitch</c> classes.
    /// </summary>
    public enum TraceLevel
    {
        /// <summary>
        /// Output no tracing and debugging messages.
        /// </summary>
        Off = 0,
        /// <summary>
        /// Output critical messages.
        /// </summary>
        Critical = 1,
        /// <summary>
        /// Output error-handling and critical messages.
        /// </summary>
        Error = 2,
        /// <summary>
        /// Output warning, error-handling and critical messages.
        /// </summary>
        Warning = 3,
        /// <summary>
        /// Output informational, warning, error-handlings and critical messages.
        /// </summary>
        Info = 4,
        /// <summary>
        /// Output debugging, informational, warning, error-handlings and critical messages.
        /// </summary>
        Debug = 5,
        /// <summary>
        /// Output tracing, debugging, informational, warning, error-handlings and critical messages.
        /// </summary>
        Trace = 6
    }
}
