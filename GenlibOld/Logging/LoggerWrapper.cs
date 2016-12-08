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
    /// </summary>
    public class LoggerWrapper : Logger
    {
        /// <summary>
        /// The dictionary that contains all the loggers.
        /// </summary>
        public Dictionary<string, Logger> Loggers { get; } = new Dictionary<string, Logger>();

        /// <summary>
        /// The function that formats the WriteException '<code>ex</code>' argument. Defaults to just '<code>ex.Message</code>'.
        /// This is overwrite the <code>ExceptionFormatter</code> in every the logger.
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

        private TraceLevel currentLevel;
        private bool individualLogSwitches = false;
        /// <summary>
        /// Whether the log switches of the individual loggers is used to log with.
        /// If set to true, then the level of the logger upon logging is used, otherwise
        /// the log level of the LoggerWrapper is used.
        /// </summary>
        public bool IndiviualLogSwitches
        {
            get { return individualLogSwitches; }
            set { individualLogSwitches = value; LogSwitch.Level = currentLevel; }
        }

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
            LogSwitch.LevelChanged += LogSwitch_LevelChanged;
        }

        private void LogSwitch_LevelChanged(TraceLevel newlevel)
        {
            currentLevel = newlevel;
            if (individualLogSwitches)
            {
                LogSwitch.LevelChanged -= LogSwitch_LevelChanged;
                LogSwitch.Level = TraceLevel.Verbose;
                LogSwitch.LevelChanged += LogSwitch_LevelChanged;
            }
        }

        private void LoggerWrapper_OnFlush(object sender, OnFlushEventArgs e)
        {
            foreach (Logger log in Loggers.Values)
                log.Flush();
        }

        private void LoggerWrapper_OnWrite(object sender, OnWriteEventArgs e)
        {
            foreach (Logger log in Loggers.Values)
                log.AppendLine(e.Written, e.Level, false, !PrefixEnabled, IndiviualLogSwitches ? log.LogSwitch.Level : LogSwitch.Level);
        }

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
    }
}
