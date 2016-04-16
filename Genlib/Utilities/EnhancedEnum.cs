using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlib.Utilities
{
    /// <summary>
    /// An enhanced enum that allows for any type of value.
    /// </summary>
    /// <typeparam name="TDecendant">The class that inherits this class.</typeparam>
    /// <typeparam name="T">The type of the enum.</typeparam>
    public class EnhancedEnum<TDecendant, T> where TDecendant : EnhancedEnum<TDecendant, T>
    {
        private T Value;

        private static Dictionary<T, EnhancedEnum<TDecendant, T>> Pointers = new Dictionary<T, EnhancedEnum<TDecendant, T>>();

        /// <summary>
        /// Creates a new enhanced enum with the specified value.
        /// </summary>
        /// <param name="value"></param>
        protected EnhancedEnum(T value)
        {
            Value = value;
            Pointers.Add(value, this);
        }

        /// <summary>
        /// Converts the enum to its value.
        /// </summary>
        /// <returns>The enum's value.</returns>
        public T ToValue()
        {
            return Value;
        }

        /// <summary>
        /// Return the enum that has that value.
        /// </summary>
        /// <param name="value">The value of the enum.</param>
        /// <returns>The enum of the corresponding value.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static TDecendant FromValue(T value)
        {
            if (!Valid(value))
                throw new ArgumentOutOfRangeException("That value is not valid");
            return (TDecendant)Pointers[value];
        }

        /// <summary>
        /// Checks of the value is valid in this enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Valid(T value) => Pointers.ContainsKey(value);

        /// <summary>
        /// Implicitly converts the enum to its value.
        /// </summary>
        /// <param name="e">The enum to convert.</param>
        public static implicit operator T(EnhancedEnum<TDecendant, T> e)
        {
            return e.Value;
        }

        /// <summary>
        /// Implicitly converts the value to its enum.
        /// </summary>
        /// <param name="val">The value to convert.</param>
        public static implicit operator EnhancedEnum<TDecendant, T>(T val)
        {
            return FromValue(val);
        }

        /// <summary>
        /// Converts the actual value of the enum to a string.
        /// </summary>
        /// <returns>The string value of the enum.</returns>
        public override string ToString()
        {
            return ToValue().ToString();
        }
    }
}
