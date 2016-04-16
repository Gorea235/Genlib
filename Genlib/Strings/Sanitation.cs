using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Genlib.Strings
{
    /// <summary>
    /// Handles the sanitation and desanitation of strings.
    /// </summary>
    public static class Sanitation
    {
        /// <summary>
        /// The type of sanitisation method to use.
        /// </summary>
        public enum SanitationType
        {
            /// <summary>
            /// The sanitation standard for XML notation.
            /// </summary>
            XML,
            /// <summary>
            /// The sanitation standard for URLs.
            /// </summary>
            URL
        }

        //private const string AlphaNumericRegex = "[0-9A-Za-z]"; // better to use full strings for regex
        private static Dictionary<SanitationType, Regex> SanitationMatchers = new Dictionary<SanitationType, Regex>()
        {
            { SanitationType.XML, new Regex("[\"&'<>]", RegexOptions.Compiled) },
            { SanitationType.URL, new Regex("[^0-9A-Za-z.\\-_~]", RegexOptions.Compiled) },
        };
        //private static Regex SanitationXMLMatch = new Regex("[\"&'<>]");
        //private static Regex SanitationURLMatch = new Regex("[^0-9A-Za-z.\\-_~]");
        private static Dictionary<string, string> XMLRawToEntity = new Dictionary<string, string>()
        {
            { "\"", "quot" },
            { "&", "amp" },
            { "'", "apos" },
            { "<", "lt" },
            { ">", "gt" },
        };
        private static Dictionary<string, string> XMLEntityToRaw = new Dictionary<string, string>()
        {
            { "quot", "\"" },
            { "amp", "&" },
            { "apos", "'" },
            { "lt", "<" },
            { "gt", ">" },
        };

        private static void PreCalculateString(string rawstring, Regex matcher, out List<string> parts, out List<int> imatches)
        {
            int indexstart = 0;
            parts = new List<string>();
            imatches = new List<int>();
            foreach (Match match in matcher.Matches(rawstring))
            {
                parts.Add(rawstring.Substring(indexstart, match.Index - indexstart));
                parts.Add(match.Value);
                imatches.Add(parts.Count - 1);
                indexstart = match.Index + match.Length;
            }
            parts.Add(rawstring.Substring(indexstart));
        }

        /// <summary>
        /// Will sanitise a string according to the standard of the sanitisation type specified.
        /// </summary>
        /// <param name="raw">The raw string to sanitise.</param>
        /// <param name="method">The method to sanitise with.</param>
        /// <returns>Sanitised string.</returns>
        public static string Sanitise(string raw, SanitationType method)
        {
            List<string> parts;
            List<int> imatches;
            PreCalculateString(raw, SanitationMatchers[method], out parts, out imatches);
            string newstr;
            if (method == SanitationType.XML)
                foreach (int i in imatches)
                {
                    if (!XMLRawToEntity.TryGetValue(parts[i], out newstr))
                        newstr = Encoding.ASCII.GetBytes(parts[i])[0].ToString("X2");
                    parts[i] = string.Format("&{0};", newstr);
                }
            else if (method == SanitationType.URL)
            {
                byte[] unibytes;
                foreach (int i in imatches)
                {
                    newstr = "";
                    unibytes = Encoding.UTF8.GetBytes(parts[i]);
                    //Array.Copy(unibytes, unibytes, unibytes.Count() - 1);
                    //Console.WriteLine(Extentions.ArrayToString(unibytes.Cast<object>().ToArray()));
                    foreach (byte unibyte in unibytes)
                        newstr += string.Format("%{0:X2}", unibyte); // unibyte.ToString("%X2");
                    //if (newstr.Substring(newstr.Length - 3) == "%00")
                    //    newstr = newstr.Substring(0, newstr.Length - 3);
                    parts[i] = newstr;
                }
            }
            return string.Join("", parts);
        }

        //public static string Sanitise(string raw, SanitationType method)
        //{
        //    string sanitised = raw;
        //    string insertstr;
        //    if (method == SanitationType.XML)
        //        foreach (Match match in SanitationXMLMatch.Matches(raw))
        //        {
        //            if (!XMLRawToEntity.TryGetValue(match.Value, out insertstr))
        //                insertstr = Encoding.ASCII.GetBytes(match.Value)[0].ToString("X2");
        //            sanitised = sanitised.Remove(match.Index, 1).Insert(match.Index, string.Format("&{0};", insertstr));
        //        }
        //    else if (method == SanitationType.URL)
        //    {
        //        byte[] unibytes;
        //        foreach (Match match in SanitationURLMatch.Matches(raw))
        //        {
        //            unibytes = Encoding.Unicode.GetBytes(match.Value);
        //            sanitised = sanitised.Remove(match.Index, 1).Insert(match.Index, insertstr); //string.Format("%{0:X2}", Encoding.ASCII.GetBytes(match.Value)[0]) //Encoding.ASCII.GetBytes(match.Value)[0].ToString("%X2")
        //        }
        //    }
        //    return sanitised;
        //}

        /// <summary>
        /// Will desanitise a sanitised string using the specified method.
        /// </summary>
        /// <param name="sanitised">The sanitised string.</param>
        /// <param name="method">The method to desanitise with.</param>
        /// <returns>The raw string.</returns>
        public static string Desanitise(string sanitised, SanitationType method)
        {
            StringBuilder raw = new StringBuilder();
            if (method == SanitationType.XML)
            {
                bool inentity = false;
                StringBuilder centity = new StringBuilder();
                string enc, scentity;
                int entitystart = -1;
                for (int i = 0; i < sanitised.Length; i++)
                {
                    if (sanitised[i] == '&' && !inentity)
                    {
                        inentity = true;
                        entitystart = i;
                    }
                    else if (sanitised[i] == ';' && inentity)
                    {
                        inentity = false;
                        scentity = centity.ToString();
                        try
                        {
                            if (!XMLEntityToRaw.TryGetValue(scentity, out enc))
                                enc = Encoding.ASCII.GetString(new byte[] { Convert.ToByte(scentity, 16) });
                        }
                        catch (Exception ex)
                        {
                            throw new FormatException(string.Format("Entity '&{0};' could not be converted.", scentity), ex);
                        }
                        raw.Append(enc);
                        centity.Clear();
                    }
                    else if ((sanitised[i] == '&' && inentity) || (sanitised[i] == ';' && !inentity))
                        throw new FormatException(string.Format("Character '{0}' is not allowed in this context", sanitised[i]));
                    else if (inentity)
                        centity.Append(sanitised[i]);
                    else
                        raw.Append(sanitised[i]);
                }
                if (inentity)
                    throw new FormatException(string.Format("Entity start at position {0} not closed", entitystart));
            }
            else if (method == SanitationType.URL)
            {
                bool inbyte = false;
                StringBuilder cbyte = new StringBuilder();
                List<byte> cbytes = new List<byte>();
                for (int i = 0; i < sanitised.Length; i++)
                {
                    if (sanitised[i] == '%' && !inbyte)
                        inbyte = true;
                    else if (inbyte)
                    {
                        cbyte.Append(sanitised[i]);
                        if (cbyte.Length == 2)
                        {
                            try
                            {
                                cbytes.Add(Convert.ToByte(cbyte.ToString(), 16));
                            }
                            catch (Exception ex)
                            {
                                throw new FormatException(string.Format("Encoded character '%{0}' could not be converted", cbyte.ToString()), ex);
                            }
                            inbyte = false;
                            cbyte.Clear();
                            if (i == sanitised.Length - 1 || sanitised[i + 1] != '%')
                            {
                                raw.Append(Encoding.UTF8.GetString(cbytes.ToArray()));
                                cbytes.Clear();
                            }
                        }
                    }
                    else
                        raw.Append(sanitised[i]);
                }
                if (inbyte)
                    throw new FormatException("Incomplete encoded character");
            }
            return raw.ToString();
        }
    }
}
