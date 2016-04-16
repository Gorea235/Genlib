using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlib.Utilities
{
    /// <summary>
    /// A small implimentation of EventArgs that has 2 fields for the old and new values of a property.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public class UpdatedPropertyEventArgs<T> : EventArgs
    {
        /// <summary>
        /// The old value of the property.
        /// </summary>
        public T OldValue;
        /// <summary>
        /// The new value of the property.
        /// </summary>
        public T NewValue;

        /// <summary>
        /// The default constructor.
        /// </summary>
        public UpdatedPropertyEventArgs() { }
        /// <summary>
        /// A constructor that provides a quick way to set the values, shortening certain lines of code.
        /// </summary>
        /// <param name="oldValue">The old value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        public UpdatedPropertyEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
