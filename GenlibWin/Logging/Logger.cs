using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genlib.Utilities;

namespace Genlib.Logging
{
    /// <summary>
    /// The base class for all loggers defined in this library.
    /// </summary>
    public abstract class Logger : IDisposable
    {

        #region fields

        #region private

        /// <summary>
        /// The queue that stores all the lines that will be written on a flush.
        /// </summary>
        private Queue<string> WriteQueue = new Queue<string>();
        private bool autoFlush = true;
        private string prefix = "";
        private bool prefixEnabled = true;
        private Func<Exception, string, string> exceptionFormatter = (Exception ex, string details) => ex.Message;

        #endregion

        #endregion

        #region events

        #region public

        /// <summary>
        /// Raised whenever the property AutoFlush is changed.
        /// </summary>
        public event EventHandler<UpdatedPropertyEventArgs<bool>> AutoFlushChanged;

        /// <summary>
        /// Raised whenever the property Prefix is changed.
        /// </summary>
        public event EventHandler<UpdatedPropertyEventArgs<string>> PrefixChanged;

        /// <summary>
        /// Raised whenever the property PrefixEnabled is changed.
        /// </summary>
        public event EventHandler<UpdatedPropertyEventArgs<bool>> PrefixEnabledChanged;

        /// <summary>
        /// Raised whenever the property ExceptionFormatter is changed.
        /// </summary>
        public event EventHandler<UpdatedPropertyEventArgs<Func<Exception, string, string>>> ExceptionFormatterChanged;

        /// <summary>
        /// Raised whenever the logger is told to flush
        /// </summary>
        public event EventHandler<OnFlushEventArgs> OnFlush;

        /// <summary>
        /// Called when the log is written to (written param includes new line)
        /// </summary>
        public event EventHandler<OnWriteEventArgs> OnWrite;

        #endregion

        #endregion

        #region properties

        #region public

        /// <summary>
        /// Whether to flush the log on every writeline.
        /// (Defaults to true)
        /// </summary>
        public bool AutoFlush
        {
            get { return autoFlush; }
            set
            {
                UpdatedPropertyEventArgs<bool> e = new UpdatedPropertyEventArgs<bool>(autoFlush, value);
                autoFlush = value;
                AutoFlushChanged?.Invoke(this, e);
            }
        }

        /// <summary>
        /// The function that formats the WriteException '<code>ex</code>' argument. Defaults to just '<code>ex.Message</code>'.
        /// </summary>
        public Func<Exception, string, string> ExceptionFormatter
        {
            get { return exceptionFormatter; }
            set
            {
                UpdatedPropertyEventArgs<Func<Exception, string, string>> e = new UpdatedPropertyEventArgs<Func<Exception, string, string>>(exceptionFormatter, value);
                exceptionFormatter = value;
                ExceptionFormatterChanged?.Invoke(this, e);
            }
        }

        /// <summary>
        /// A string added to the front of each line as it's written after going through string.Format, with
        /// {0} being formatted as the current DataTime, {1} being formatted as the current TraceLevel LevelName,
        /// {2} being formatted as the TraceSwitch DisplayName and {3} being formatted as the TraceSwitch
        /// Description.
        /// (e.g. 'Log [{0:yyyy-MM-dd}] [{0:HH:mm:ss}] [Level: {1}] ').
        /// </summary>
        public string Prefix
        {
            get { return prefix; }
            set
            {
                UpdatedPropertyEventArgs<string> e = new UpdatedPropertyEventArgs<string>(prefix, value);
                prefix = value;
                PrefixChanged?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Whether the prefix is enabled for the logger.
        /// </summary>
        public bool PrefixEnabled
        {
            get { return prefixEnabled; }
            set
            {
                UpdatedPropertyEventArgs<bool> e = new UpdatedPropertyEventArgs<bool>(prefixEnabled, value);
                prefixEnabled = value;
                PrefixEnabledChanged?.Invoke(this, e);
            }
        }

        /// <summary>
        /// The TraceSwitch that can be used to decide on what to log.
        /// Defaults to <c>TraceSwitch("Log", "Log") { Level = TraceLevel.Info }</c>.
        /// If something is logged at a higher level than the level in the TraceSwitch,
        /// then it is not logged.
        /// </summary>
        public TraceSwitch Switch { get; set; } = new TraceSwitch("Log", "Log", TraceLevel.Info);

        #endregion

        #endregion

        #region classes

        #region public

        /// <summary>
        /// The event args for the OnFlush event.
        /// </summary>
        public class OnFlushEventArgs : EventArgs
        {
            /// <summary>
            /// The full string, including newline, that was flushed to the logger's output.
            /// </summary>
            public string FullString;
        }

        /// <summary>
        /// The event args for the PrefixEnabledChanged event.
        /// </summary>
        public class OnWriteEventArgs : EventArgs
        {
            /// <summary>
            /// The fully formatted string that was written.
            /// </summary>
            public string Written;
            /// <summary>
            /// The level that it was written at.
            /// </summary>
            public TraceLevel Level;
        }

        #endregion

        #endregion

        #region methods

        #region public

        /// <summary>
        /// Flushes the contents of the log to any functions connected to the OnFlush event.
        /// </summary>
        public void Flush()
        {
            string fullstring = "";
            while (WriteQueue.Count > 0)
                fullstring += WriteQueue.Dequeue();
            OnFlush?.Invoke(this, new OnFlushEventArgs() { FullString = fullstring });
        }

        /// <summary>
        /// Formats and appends a line onto the log.
        /// </summary>
        /// <param name="line">The line to write.</param>
        /// <param name="level">The level to write it at (if higher than current trace level, then the append is ignored).</param>
        public void AppendLine(string line, TraceLevel level)
        {
            if (level > Switch.Level)
                return;
            try { line = string.Format(Prefix, DateTime.Now, Switch.CurrentLevelName, Switch.DisplayName, Switch.Description) + line; }
            catch (Exception ex) { line = string.Format("[Log prefix formatting failed, reason: {0}] {1}", ex.Message, line); }
            WriteQueue.Enqueue(line);
            OnWrite?.Invoke(this, new OnWriteEventArgs() { Written = line, Level = level });
            if (AutoFlush)
                Flush();
        }

        /// <summary>
        /// Writes a string into the log with the TraceLevel as Error.
        /// </summary>
        /// <param name="str">The string to write.</param>
        public void WriteError(string str) => AppendLine(str + Environment.NewLine, TraceLevel.Error);

        /// <summary>
        /// Writes a string into the log with the TraceLevel as Error, formatted with <c>System.String.Format</c>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objs">An optional param list of objects.</param>
        public void WriteError(string format, params object[] objs) => WriteError(string.Format(format, objs));

        /// <summary>
        /// Writes a string into the log with the TraceLevel as Warning.
        /// </summary>
        /// <param name="str">The string to write.</param>
        public void WriteWarning(string str) => AppendLine(str + Environment.NewLine, TraceLevel.Warning);

        /// <summary>
        /// Writes a string into the log with the TraceLevel as Warning, formatted with <c>System.String.Format</c>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objs">An optional param list of objects.</param>
        public void WriteWarning(string format, params object[] objs) => WriteWarning(string.Format(format, objs));

        /// <summary>
        /// Writes a string into the log with the TraceLevel as Info.
        /// </summary>
        /// <param name="str">The string to write.</param>
        public void WriteInfo(string str) => AppendLine(str + Environment.NewLine, TraceLevel.Info);

        /// <summary>
        /// Writes a string into the log with the TraceLevel as Info, formatted with <c>System.String.Format</c>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objs">An optional param list of objects.</param>
        public void WriteInfo(string format, params object[] objs) => WriteInfo(string.Format(format, objs));

        /// <summary>
        /// Writes a string into the log with the TraceLevel as Debug.
        /// </summary>
        /// <param name="str">The string to write.</param>
        public void WriteDebug(string str) => AppendLine(str + Environment.NewLine, TraceLevel.Debug);

        /// <summary>
        /// Writes a string into the log with the TraceLevel as Debug, formatted with <c>System.String.Format</c>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objs">An optional param list of objects.</param>
        public void WriteDebug(string format, params object[] objs) => WriteDebug(string.Format(format, objs));

        /// <summary>
        /// Writes a string into the log with the TraceLevel as Debug.
        /// </summary>
        /// <param name="str">The string to write.</param>
        public void WriteTrace(string str) => AppendLine(str + Environment.NewLine, TraceLevel.Trace);

        /// <summary>
        /// Writes a string into the log with the TraceLevel as Trace, formatted with <c>System.String.Format</c>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objs">An optional param list of objects.</param>
        public void WriteTrace(string format, params object[] objs) => WriteTrace(string.Format(format, objs));

        /// <summary>
        /// Logs an exception that is formatted using <code>Logger.ExceptionFormatter</code>.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="details">The custom details of the exception.</param>
        /// <param name="level">The level to log the exception at.</param>
        public void WriteException(Exception ex, string details = "", TraceLevel level = TraceLevel.Error) => AppendLine(ExceptionFormatter(ex, details) + Environment.NewLine, level);

        /// <summary>
        /// Closes the logger.
        /// </summary>
        public abstract void Close();
        /// <summary>
        /// Disposes of the logger.
        /// </summary>
        public abstract void Dispose();

        #endregion

        #endregion

    }
}
