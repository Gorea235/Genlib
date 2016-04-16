using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Genlib.Utilities
{
    /// <summary>
    /// Contains extention methods that aid in handling data types (some extentions are contained in their relevant class).
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// An extension method that converts an IEnumerable into a string.
        /// </summary>
        /// <param name="enu">The IEnumerable to convert.</param>
        /// <returns>String representation of the IEnumerable.</returns>
        public static string ToArrayString(this System.Collections.IEnumerable enu)
        {
            string ret = "{";
            foreach (object o in enu)
            {
                if (o.GetType() == typeof(string))
                    ret += "\"" + o + "\", ";
                else if (o.GetType() == typeof(char))
                    ret += "'" + o.ToString() + "', ";
                else if ((o as System.Collections.IEnumerable) != null)
                    ret += ToArrayString((System.Collections.IEnumerable)o) + ", ";
                else
                    ret += o.ToString() + ", ";
            }
            if (ret.Length > 1)
                ret = ret.Substring(0, ret.Length - 2);
            ret += "}";
            return ret;
        }

        /// <summary>
        /// Converts an array of KeyValuePairs to a Dictionary.
        /// </summary>
        /// <typeparam name="T1">The first type.</typeparam>
        /// <typeparam name="T2">The second type.</typeparam>
        /// <param name="kvs">The array of KeyValuePairs</param>
        /// <returns></returns>
        public static Dictionary<T1, T2> ToDictionary<T1, T2>(this IEnumerable<KeyValuePair<T1, T2>> kvs)
        {
            Dictionary<T1, T2> dict = new Dictionary<T1, T2>();
            foreach (KeyValuePair<T1, T2> kv in kvs)
                dict.Add(kv.Key, kv.Value);
            return dict;
        }

        /// <summary>
        /// Gets the description of the enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="en">The value of the enum to get the description of.</param>
        /// <returns>The description of the enum, or, if it fails, the ToString value of it.</returns>
        public static string GetDescription<T>(this T en) where T : struct
        {
            Type t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentNullException("Value must be an Enum.");
            MemberInfo[] mInfo = t.GetMember(en.ToString());
            if (mInfo != null && mInfo.Length > 0)
            {
                try
                {
                    Attribute attr = mInfo[0].GetCustomAttribute(typeof(DescriptionAttribute), false);

                    if (attr != null)
                        return (attr as DescriptionAttribute).Description;
                }
                catch { }
            }
            return en.ToString();
        }
    }
}
