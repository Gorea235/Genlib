#if DEBUG && !NET_CORE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Genlib.Utilities
{
    /// <summary>
    /// Functions to deal with checking and handling emails.
    /// </summary>
    public static class Email
    {
        /// <summary>
        /// Code taken from the vs100 MSDN page detailing how to verify emails (for test purposes)
        /// </summary>
        /// <param name="strIn">The string to check</param>
        /// <returns>Whether the string was a valid email</returns>
        /// <remarks>Debug only ( https://msdn.microsoft.com/en-us/library/01escwtf(v=vs.100).aspx )</remarks>
        public static bool IsValidEmail_vs100(string strIn)
        {
            bool invalid = false;
            if (string.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            strIn = Regex.Replace(strIn, @"(@)(.+)$", (Match match) =>
            {
                // IdnMapping class with default property values.
                IdnMapping idn = new IdnMapping();

                string domainName = match.Groups[2].Value;
                try
                {
                    domainName = idn.GetAscii(domainName);
                }
                catch (ArgumentException)
                {
                    invalid = true;
                }
                return match.Groups[1].Value + domainName;
            });
            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                   RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Code taken from the vs110 MSDN page detailing how to verify emails (for test purposes)
        /// </summary>
        /// <param name="strIn">The string to check</param>
        /// <returns>Whether the string was a valid email</returns>
        /// <remarks>Debug only ( https://msdn.microsoft.com/en-us/library/01escwtf(v=vs.110).aspx )</remarks>
        public static bool IsValidEmail_vs110(string strIn)
        {
            bool invalid = false;
            if (string.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", (Match match) =>
                {
                    // IdnMapping class with default property values.
                    IdnMapping idn = new IdnMapping();

                    string domainName = match.Groups[2].Value;
                    try
                    {
                        domainName = idn.GetAscii(domainName);
                    }
                    catch (ArgumentException)
                    {
                        invalid = true;
                    }
                    return match.Groups[1].Value + domainName;
                },
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
#endif