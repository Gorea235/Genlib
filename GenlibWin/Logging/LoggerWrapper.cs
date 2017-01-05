using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlib.Logging
{
    /// <summary>
    /// Allows for multiple loggers to be managed at once.
    /// Properties that are set in the class are pushed to
    /// all current logs, however they are not enforced,
    /// meaning that if the properties of an individual
    /// logger are changed, they will be kept and used when
    /// writing.
    /// </summary>
    public class LoggerWrapper : Logger
    {

        #region properties

        #region public

        /// <summary>
        /// The dictionary that contains all the loggers.
        /// </summary>
        public Dictionary<string, Logger> Loggers { get; } = new Dictionary<string, Logger>();

        /// <summary>
        /// The function that formats the WriteException '<c>ex</c>' argument. Defaults to just '<code>ex.Message</code>'.
        /// This is overwrite the <c>ExceptionFormatter</c> in every the logger.
        /// </summary>
        public new Func<Exception, string, string> ExceptionFormatter
        {
            get { return base.ExceptionFormatter; }
            set
            {
                base.ExceptionFormatter = value;
                foreach (Logger logger in Loggers.Values)
                    logger.ExceptionFormatter = value;
            }
        }

        #endregion

        #endregion

        #region constructors

        /// <summary>
        /// Constructs a new LoggerWrapper instance.
        /// </summary>
        /// <param name="logs">Logs to add into the dictionary.</param>
        public LoggerWrapper(params KeyValuePair<string, Logger>[] logs)
        {
            foreach (KeyValuePair<string, Logger> log in logs)
                Loggers.Add(log.Key, log.Value);
            OnWrite += LoggerWrapper_OnWrite;
            OnFlush += LoggerWrapper_OnFlush;

            AutoFlushChanged += LoggerWrapper_AutoFlushChanged;
            ExceptionFormatterChanged += LoggerWrapper_ExceptionFormatterChanged;
            PrefixChanged += LoggerWrapper_PrefixChanged;
            PrefixEnabledChanged += LoggerWrapper_PrefixEnabledChanged;
            Switch.LevelChanged += LogSwitch_LevelChanged;
        }

        #endregion

        #region methods

        #region private

        private void LoggerWrapper_AutoFlushChanged(object sender, Utilities.UpdatedPropertyEventArgs<bool> e)
        {
            foreach (Logger log in Loggers.Values)
                log.AutoFlush = e.NewValue;
        }

        private void LoggerWrapper_ExceptionFormatterChanged(object sender, Utilities.UpdatedPropertyEventArgs<Func<Exception, string, string>> e)
        {
            foreach (Logger log in Loggers.Values)
                log.ExceptionFormatter = e.NewValue;
        }

        private void LogSwitch_LevelChanged(object sender, Utilities.UpdatedPropertyEventArgs<TraceLevel> e)
        {
            foreach (Logger log in Loggers.Values)
                log.Switch.Level = e.NewValue;
        }

        private void LoggerWrapper_PrefixChanged(object sender, Utilities.UpdatedPropertyEventArgs<string> e)
        {
            foreach (Logger log in Loggers.Values)
                log.Prefix = e.NewValue;
        }

        private void LoggerWrapper_PrefixEnabledChanged(object sender, Utilities.UpdatedPropertyEventArgs<bool> e)
        {
            foreach (Logger log in Loggers.Values)
                log.PrefixEnabled = e.NewValue;
        }

        private void LoggerWrapper_OnFlush(object sender, OnFlushEventArgs e)
        {
            foreach (Logger log in Loggers.Values)
                log.Flush();
        }

        private void LoggerWrapper_OnWrite(object sender, OnWriteEventArgs e)
        {
            foreach (Logger log in Loggers.Values)
                log.AppendLine(e.Written, e.Level);
        }

        #endregion

        #region public

        /// <summary>
        /// Closes the logger
        /// </summary>
        public override void Close()
        {
            foreach (Logger log in Loggers.Values)
                log.Close();
        }

        /// <summary>
        /// Disposes the logger.
        /// </summary>
        public override void Dispose()
        {
            foreach (Logger log in Loggers.Values)
                log.Dispose();
        }

        #endregion

        #endregion

    }
}
