#if !NETCOREAPP1_1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Genlib.Utilities
{
    /// <summary>
    /// Specific colour functions.
    /// </summary>
    public static class ColorEx
    {
        private static Regex HexColour = new Regex("^([0-9A-F]{8})|([0-9A-F]{6})$", RegexOptions.IgnoreCase & RegexOptions.Compiled);
        /// <summary>
        /// Converts a 6 or 8 digit hex code to a color.
        /// </summary>
        /// <param name="code">The 6 or 8 digit code.</param>
        /// <returns></returns>
        public static System.Windows.Media.Color FromString(string code)
        {
            if (code[0] == '#')
                code = code.ToUpper().Substring(1);
            if (!HexColour.IsMatch(code) || code.Length > 8)
                throw new ArgumentException("Hex string not in correct format");
            if (code.Length < 8)
                code = "FF" + code;
            return new System.Windows.Media.Color()
            {
                R = Convert.ToByte(code.Substring(2, 2), 16),
                G = Convert.ToByte(code.Substring(4, 2), 16),
                B = Convert.ToByte(code.Substring(6, 2), 16),
                A = Convert.ToByte(code.Substring(0, 2), 16)
            };
        }
    }
}
#endif