using System;
using System.Collections.Generic;
using System.Text;

namespace Genlib.Utilities
{
    [Flags]
    public enum EmailOptions
    {
        /// <summary>
        /// Indicates default options.
        /// </summary>
        None,
        /// <summary>
        /// Converts email to lowercase when parsing.
        /// </summary>
        IgnoreCase,
        /// <summary>
        /// Removes all periods from local part when parsing.
        /// If quotes are allowed, this could cause exceptions to
        /// occur is the quote is preceded or followed by a dot.
        /// </summary>
        StripDots,
        /// <summary>
        /// Removed all special chars than are allowed in the local
        /// part by default when parsing.
        /// </summary>
        StripSpecialChars,
        /// <summary>
        /// Causes the parser to fail if the email contains comments,
        /// regardless of if they are valid.
        /// </summary>
        DisallowComments,
        /// <summary>
        /// Causes the parser to fail if the local part contains
        /// specical characters, regardless of if they are valid.
        /// </summary>
        DisallowSpecialChars,
        /// <summary>
        /// Causes the parser to fail if the local part contains quoets,
        /// regardless of if they are valid.
        /// </summary>
        DisallowQuotes,
        /// <summary>
        /// Causes the parser to fail if the local part contains a quote
        /// that contains escaped characters, regardless of if they are
        /// valid.
        /// </summary>
        DisallowCharEscaping
    }
}
