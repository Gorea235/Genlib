using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genlib.Utilities;

namespace Genlib.Logging
{
    /// <summary>
    /// A class that inherits the class TraceSwitch, but adds events to the properties.
    /// </summary>
    public class TraceSwitch
    {

        #region fields

        #region private

        private TraceLevel level;

        #endregion

        #endregion

        #region events

        #region public

        /// <summary>
        /// Raised whenever the level is changed.
        /// </summary>
        public event EventHandler<UpdatedPropertyEventArgs<TraceLevel>> LevelChanged;

        #endregion

        #endregion

        #region properties

        #region public

        /// <summary>
        /// Gets the string name of the current TraceLevel.
        /// </summary>
        public string CurrentLevelName { get { return GetLevelName(Level); } }

        /// <summary>
        /// The description of the switch.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The display name of the switch.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the trace level that determines the message the switch allows.
        /// </summary>
        public TraceLevel Level
        {
            get { return level; }
            set
            {
                UpdatedPropertyEventArgs<TraceLevel> e = new UpdatedPropertyEventArgs<TraceLevel>(level, value);
                level = value;
                LevelChanged?.Invoke(this, e);
            }
        }

        /// <summary>
        /// The string names of the trace levels.
        /// </summary>
        public Dictionary<TraceLevel, string> TraceLevelStrings { get; set; } = new Dictionary<TraceLevel, string>()
        {
            { TraceLevel.Critical, "crit" },
            { TraceLevel.Error, "fail" },
            { TraceLevel.Warning, "warn" },
            { TraceLevel.Info, "info" },
            { TraceLevel.Debug, "dbug" },
            { TraceLevel.Trace, "trce" },
        };

        /// <summary>
        /// Indicates whether the switch can output cirtical messages.
        /// </summary>
        /// <remarks>
        /// Outputs true when Level is
        /// <c>Genlib.Logging.TraceLevel.Critical</c>,
        /// <c>Genlib.Logging.TraceLevel.Error</c>,
        /// <c>Genlib.Logging.TraceLevel.Warning</c>,
        /// <c>Genlib.Logging.TraceLevel.Info</c>,
        /// <c>Genlib.Logging.TraceLevel.Debug</c>, or
        /// <c>Genlib.Logging.TraceLevel.Trace</c>.
        /// </remarks>
        public bool TraceCritical { get { return Level >= TraceLevel.Critical; } }

        /// <summary>
        /// Indicates whether the switch can output error messages.
        /// </summary>
        /// <remarks>
        /// Outputs true when Level is
        /// <c>Genlib.Logging.TraceLevel.Error</c>,
        /// <c>Genlib.Logging.TraceLevel.Warning</c>,
        /// <c>Genlib.Logging.TraceLevel.Info</c>,
        /// <c>Genlib.Logging.TraceLevel.Debug</c>, or
        /// <c>Genlib.Logging.TraceLevel.Trace</c>.
        /// </remarks>
        public bool TraceError { get { return Level >= TraceLevel.Error; } }

        /// <summary>
        /// Indicates whether the switch can output warning messages.
        /// </summary>
        /// <remarks>
        /// Outputs true when Level is
        /// <c>Genlib.Logging.TraceLevel.Warning</c>,
        /// <c>Genlib.Logging.TraceLevel.Info</c>,
        /// <c>Genlib.Logging.TraceLevel.Debug</c>, or
        /// <c>Genlib.Logging.TraceLevel.Trace</c>.
        /// </remarks>
        public bool TraceWarning { get { return Level >= TraceLevel.Warning; } }

        /// <summary>
        /// Indicates whether the switch can output infomation messages.
        /// </summary>
        /// <remarks>
        /// Outputs true when Level is
        /// <c>Genlib.Logging.TraceLevel.Info</c>,
        /// <c>Genlib.Logging.TraceLevel.Debug</c>, or
        /// <c>Genlib.Logging.TraceLevel.Trace</c>.
        /// </remarks>
        public bool TraceInfo { get { return Level >= TraceLevel.Info; } }

        /// <summary>
        /// Indicates whether the switch can output debug messages.
        /// </summary>
        /// <remarks>
        /// Outputs true when Level is
        /// <c>Genlib.Logging.TraceLevel.Debug</c>, or
        /// <c>Genlib.Logging.TraceLevel.Trace</c>.
        /// </remarks>
        public bool TraceDebug { get { return Level >= TraceLevel.Debug; } }

        /// <summary>
        /// Indicates whether the switch can output trace messages.
        /// </summary>
        /// <remarks>
        /// Outputs true when Level is
        /// <c>Genlib.Logging.TraceLevel.Trace</c>.
        /// </remarks>
        public bool TraceTrace { get { return Level >= TraceLevel.Trace; } }

        #endregion

        #endregion

        #region constructors

        #region public

        /// <summary>
        /// Creates a new TraceSwitch that has events.
        /// </summary>
        /// <param name="displayName">The name to display on a user interface.</param>
        /// <param name="description">The description of the switch</param>
        public TraceSwitch(string displayName, string description)
        {
            DisplayName = displayName;
            Description = description;
        }

        /// <summary>
        /// Creates a new TraceSwitch that has events.
        /// </summary>
        /// <param name="displayName">The name to display on a user interface.</param>
        /// <param name="description">The description of the switch</param>
        /// <param name="level">The level to start the logger at.</param>
        public TraceSwitch(string displayName, string description, TraceLevel level)
        {
            DisplayName = displayName;
            Description = description;
            Level = level;
        }

        #endregion

        #endregion

        #region methods

        #region public

        /// <summary>
        /// Gets the string name of the given level.
        /// </summary>
        /// <param name="level">The level to get the name of.</param>
        /// <returns>The name of the level.</returns>
        public string GetLevelName(TraceLevel level) => TraceLevelStrings.ContainsKey(level) ? TraceLevelStrings[level] : level.ToString();

        #endregion

        #endregion
    }
}
