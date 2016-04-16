using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Genlib.Utilities;

namespace Genlib.Logging
{
    /// <summary>
    /// A Logger based wrapper around the System.Diagnostics.Debug logging method
    /// </summary>
    public class DebugLogger : Logger
    {
        /// <summary>
        /// Creates a new logger that writes to the visual studio debug log.
        /// </summary>
        public DebugLogger()
        {
            OnWrite += DebugLogger_OnWrite;
            OnFlush += DebugLogger_OnFlush;
            AutoFlushChanged += DebugLogger_AutoFlushChanged;
        }

        private void DebugLogger_AutoFlushChanged(object sender, UpdatedPropertyEventArgs<bool> e)
        {
            Debug.AutoFlush = e.NewValue;
        }

        private void DebugLogger_OnFlush(object sender, OnFlushEventArgs e)
        {
            Debug.Flush();
        }

        private void DebugLogger_OnWrite(object sender, OnWriteEventArgs e)
        {
            Debug.Write(e.Written);
        }

        /// <summary>
        /// Closes the logger.
        /// </summary>
        public override void Close() { }
        /// <summary>
        /// Disposes the logger.
        /// </summary>
        public override void Dispose() { }
    }
}
