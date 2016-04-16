using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlib.Logging
{
    /// <summary>
    /// A class that inherits the class TraceSwitch, but adds events to the properties.
    /// </summary>
    public class EventedTraceSwitch : TraceSwitch
    {
        /// <summary>
        /// Creates a new TraceSwitch that has events.
        /// </summary>
        /// <param name="displayName">The name to display on a user interface.</param>
        /// <param name="description">The description of the switch</param>
        public EventedTraceSwitch(string displayName, string description) : base(displayName, description) { }
        /// <summary>
        /// Creates a new TraceSwitch that has events.
        /// </summary>
        /// <param name="displayName">The name to display on a user interface.</param>
        /// <param name="description">The description of the switch</param>
        /// <param name="defaultSwitchValue">The defaul value.</param>
        public EventedTraceSwitch(string displayName, string description, string defaultSwitchValue) : base(displayName, description, defaultSwitchValue) { }

        /// <summary>
        /// Gets or sets the trace level that determines the message the switch allows.
        /// </summary>
        public new TraceLevel Level
        {
            get { return base.Level; }
            set { base.Level = value; LevelChanged?.Invoke(value); }
        }
        /// <summary>
        /// The delegate for the LevelChanged event.
        /// </summary>
        /// <param name="newlevel"></param>
        public delegate void LevelChangedDelegate(TraceLevel newlevel);
        /// <summary>
        /// Raised whenever the level is changed.
        /// </summary>
        public event LevelChangedDelegate LevelChanged;

        /// <summary>
        /// Gets or sets the current setting for this switch.
        /// </summary>
        public new int SwitchSetting
        {
            get { return base.SwitchSetting; }
            set { SwitchSettingChanged?.Invoke(value); base.SwitchSetting = value; }
        }
        /// <summary>
        /// The delegate for the <c>SwitchSettingChanged</c> event.
        /// </summary>
        /// <param name="newSwitchSetting"></param>
        public delegate void SwitchSettingChangedDelegate(int newSwitchSetting);
        /// <summary>
        /// Raised whenever the SwitchSetting is changed.
        /// </summary>
        public event SwitchSettingChangedDelegate SwitchSettingChanged;
    }
}
