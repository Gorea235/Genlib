using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Genlib.Strings
{
    /// <summary>
    /// Provides functions for handling strings with certain formats.
    /// </summary>
    class Formatting
    {
        private const string RegexUriStr = @"
(?ix)
# Regex encoding for the URI format defined in
# https://tools.ietf.org/html/rfc3986#appendix-A
# The regex here is derived exactly from the definition.
# Regex does not support the Backus-Naur form named
# capture groups, so the segments have to be repeated.
# Does not support scheme or platform specific restrictions.
^
(?: # scheme
    [A-Za-z] [[:alpha:][:digit:]+.-]*
)
: # required seperator
(?: # hier-part
    \/\/
    (?:
        (?:
            (?: # authority
                (?:
                    (?: # userinfo
                        (?: [[:alpha:][:digit:]._~-] |
                            (?: % [[:xdigit:]]{2} ) |
                            (?: [!$&'()*+,;=] ) | :
                        )*
                    )
                    @
                )?
                (?: # host
                    (?: # IP-literal
                        \[
                        (?:
                            (?: # IPv6address
                                (?: # '6( h16 "":"" ) ls32'
                                    (?: [[:xdigit:]]{1,4} : ){6}
                                    (?: # ls32
                                        (?: [[:xdigit:]]{1,4} : [[:xdigit:]]{1,4} ) |
                                        (?: # IPv4address
                                            (?: 
                                                (?: [[:digit:]] |
                                                    [1-9]
        [[:digit:]] |
                                                    1 [[:digit:]]{2} |
                                                    2 [0-4]
        [[:digit:]] |
                                                    2 5 [0-5]
                                                )
                                                \.
                                            ){3}
                                            (?: [[:digit:]] |
                                                [1-9]
        [[:digit:]] |
                                                1 [[:digit:]]{2} |
                                                2 [0-4]
        [[:digit:]] |
                                                2 5 [0-5]
                                            )
                                        ) # IPv4address
                                    ) # ls32
                                ) # '6( h16 "":"" ) ls32'
                                |
                                (?: # '""::"" 5( h16 "":"" ) ls32'
                                    ::
                                    (?: [[:xdigit:]]{1,4} : ){5}
                                    (?: # ls32
                                        (?: [[:xdigit:]]{1,4} : [[:xdigit:]]{1,4} ) |
                                        (?: # IPv4address
                                            (?: 
                                                (?: [[:digit:]] |
                                                    [1-9]
        [[:digit:]] |
                                                    1 [[:digit:]]{2} |
                                                    2 [0-4]
        [[:digit:]] |
                                                    2 5 [0-5]
                                                )
                                                \.
                                            ){3}
                                            (?: [[:digit:]] |
                                                [1-9]
        [[:digit:]] |
                                                1 [[:digit:]]{2} |
                                                2 [0-4]
        [[:digit:]] |
                                                2 5 [0-5]
                                            )
                                        ) # IPv4address
                                    ) # ls32
                                ) # '""::"" 5( h16 "":"" ) ls32'
                                |
                                (?: # '[ h16 ] ""::"" 4( h16 "":"" ) ls32'
                                    (?: [[:xdigit]]{1,4} )?
                                    ::
                                    (?: [[:xdigit:]]{1,4} : ){4}
                                    (?: # ls32
                                        (?: [[:xdigit:]]{1,4} : [[:xdigit:]]{1,4} ) |
                                        (?: # IPv4address
                                            (?: 
                                                (?: [[:digit:]] |
                                                    [1-9]
        [[:digit:]] |
                                                    1 [[:digit:]]{2} |
                                                    2 [0-4]
        [[:digit:]] |
                                                    2 5 [0-5]
                                                )
                                                \.
                                            ){3}
                                            (?: [[:digit:]] |
                                                [1-9]
        [[:digit:]] |
                                                1 [[:digit:]]{2} |
                                                2 [0-4]
        [[:digit:]] |
                                                2 5 [0-5]
                                            )
                                        ) # IPv4address
                                    ) # ls32
                                ) # '[ h16 ] ""::"" 4( h16 "":"" ) ls32'
                                |
                                (?: # '[ *1( h16 "":"" ) h16 ] ""::"" 3( h16 "":"" ) ls32'
                                    (?: (?: [[:xdigit:]] : ){0,1} [[:xdigit]]{1,4} )?
                                    ::
                                    (?: [[:xdigit:]]{1,4} : ){3}
                                    (?: # ls32
                                        (?: [[:xdigit:]]{1,4} : [[:xdigit:]]{1,4} ) |
                                        (?: # IPv4address
                                            (?: 
                                                (?: [[:digit:]] |
                                                    [1-9]
        [[:digit:]] |
                                                    1 [[:digit:]]{2} |
                                                    2 [0-4]
        [[:digit:]] |
                                                    2 5 [0-5]
                                                )
                                                \.
                                            ){3}
                                            (?: [[:digit:]] |
                                                [1-9]
        [[:digit:]] |
                                                1 [[:digit:]]{2} |
                                                2 [0-4]
        [[:digit:]] |
                                                2 5 [0-5]
                                            )
                                        ) # IPv4address
                                    ) # ls32
                                ) # '[ *1( h16 "":"" ) h16 ] ""::"" 3( h16 "":"" ) ls32'
                                |
                                (?: # '[ *2( h16 "":"" ) h16 ] ""::"" 2( h16 "":"" ) ls32'
                                    (?: (?: [[:xdigit:]] : ){0,2} [[:xdigit]]{1,4} )?
                                    ::
                                    (?: [[:xdigit:]]{1,4} : ){2}
                                    (?: # ls32
                                        (?: [[:xdigit:]]{1,4} : [[:xdigit:]]{1,4} ) |
                                        (?: # IPv4address
                                            (?: 
                                                (?: [[:digit:]] |
                                                    [1-9]
        [[:digit:]] |
                                                    1 [[:digit:]]{2} |
                                                    2 [0-4]
        [[:digit:]] |
                                                    2 5 [0-5]
                                                )
                                                \.
                                            ){3}
                                            (?: [[:digit:]] |
                                                [1-9]
        [[:digit:]] |
                                                1 [[:digit:]]{2} |
                                                2 [0-4]
        [[:digit:]] |
                                                2 5 [0-5]
                                            )
                                        ) # IPv4address
                                    ) # ls32
                                ) # '[ *2( h16 "":"" ) h16 ] ""::"" 2( h16 "":"" ) ls32'
                                |
                                (?: # '[ *3( h16 "":"" ) h16 ] ""::"" h16 "":"" ls32'
                                    (?: (?: [[:xdigit:]] : ){0,3} [[:xdigit]]{1,4} )?
                                    ::
                                    (?: [[:xdigit:]]{1,4} : )
                                    (?: # ls32
                                        (?: [[:xdigit:]]{1,4} : [[:xdigit:]]{1,4} ) |
                                        (?: # IPv4address
                                            (?: 
                                                (?: [[:digit:]] |
                                                    [1-9]
        [[:digit:]] |
                                                    1 [[:digit:]]{2} |
                                                    2 [0-4]
        [[:digit:]] |
                                                    2 5 [0-5]
                                                )
                                                \.
                                            ){3}
                                            (?: [[:digit:]] |
                                                [1-9]
        [[:digit:]] |
                                                1 [[:digit:]]{2} |
                                                2 [0-4]
        [[:digit:]] |
                                                2 5 [0-5]
                                            )
                                        ) # IPv4address
                                    ) # ls32
                                ) # '[ *3( h16 "":"" ) h16 ] ""::"" h16 "":"" ls32'
                                |
                                (?: # '[ *4( h16 "":"" ) h16 ] ""::"" ls32'
                                    (?: (?: [[:xdigit:]] : ){0,4} [[:xdigit]]{1,4} )?
                                    ::
                                    (?: # ls32
                                        (?: [[:xdigit:]]{1,4} : [[:xdigit:]]{1,4} ) |
                                        (?: # IPv4address
                                            (?: 
                                                (?: [[:digit:]] |
                                                    [1-9]
        [[:digit:]] |
                                                    1 [[:digit:]]{2} |
                                                    2 [0-4]
        [[:digit:]] |
                                                    2 5 [0-5]
                                                )
                                                \.
                                            ){3}
                                            (?: [[:digit:]] |
                                                [1-9]
        [[:digit:]] |
                                                1 [[:digit:]]{2} |
                                                2 [0-4]
        [[:digit:]] |
                                                2 5 [0-5]
                                            )
                                        ) # IPv4address
                                    ) # ls32
                                ) # '[ *4( h16 "":"" ) h16 ] ""::"" ls32'
                                |
                                (?: # '[ *5( h16 "":"" ) h16 ] ""::"" h16'
                                    (?: (?: [[:xdigit:]] : ){0,5} [[:xdigit]]{1,4} )?
                                    ::
                                    (?: [[:xdigit:]]{1,4} )
                                ) # '[ *5( h16 "":"" ) h16 ] ""::"" h16'
                                |
                                (?: # '[ *6( h16 "":"" ) h16 ] ""::""'
                                    (?: (?: [[:xdigit:]] : ){0,6} [[:xdigit]]{1,4} )?
                                    ::
                                ) # '[ *6( h16 "":"" ) h16 ] ""::""'
                            ) # IPv6address
                            |
                            (?: # IPvFuture
                                v[[:xdigit:]]+
                                \.
                                (?: 
                                    [[:alpha:]
        [:digit:]._ ~-] |
                                    [!$&'()*+,;=] | 
                                    :
                                )?
                            ) # IPvFuture
                        )
                        \]
                    ) # IP-literal
                    |
                    (?: # IPv4address
                        (?: 
                            (?: [[:digit:]] |
                                [1-9]
        [[:digit:]] |
                                1 [[:digit:]]{2} |
                                2 [0-4]
        [[:digit:]] |
                                2 5 [0-5]
                            )
                            \.
                        ){3}
                        (?: [[:digit:]] |
                            [1-9]
        [[:digit:]] |
                            1 [[:digit:]]{2} |
                            2 [0-4]
        [[:digit:]] |
                            2 5 [0-5]
                        )
                    ) # IPv4address
                    |
                    (?: # reg-name
                        (?: [[:alpha:]
        [:digit:]._ ~-] |
                            (?: % [[:xdigit:]]{2} ) |
                            (?: [!$&'()*+,;=] )
                        )*
                    ) # reg-name
                ) # host
                (?:
                    :
                    (?: # port
                        [[:digit:]]+
                    )
                )?
            ) # authority
            (?: # path-adempty
                (?:
                    \/
                    (?: # pchar
                        (?:
                            [[:alpha:]
        [:digit:]._ ~-] |
                            (?: % [[:xdigit:]]{2} ) |
                            (?: [!$&'()*+,;=] ) |
                            : |
                            @
                        )
                    )*
                )*
            ) # path-adempty
        )
        |
        (?: # path-absolute
            \/
            (?:
                (?: # pchar
                    (?:
                        [[:alpha:]
        [:digit:]._ ~-] |
                        (?: % [[:xdigit:]]{2} ) |
                        (?: [!$&'()*+,;=] ) |
                        : |
                        @
                    )
                )+
                (?:
                    \/
                    (?: # pchar
                        (?:
                            [[:alpha:]
        [:digit:]._ ~-] |
                            (?: % [[:xdigit:]]{2} ) |
                            (?: [!$&'()*+,;=] ) |
                            : |
                            @
                        )
                    )*
                )*
            )?
        )
        |
        (?: # path-noscheme
            (?: # segment-nz-nc
                (?:
                    [[:alpha:]
        [:digit:]._ ~-] |
                    (?: % [[:xdigit:]]{2} ) |
                    (?: [!$&'()*+,;=] ) |
                    @
                )
            )+
            (?:
                \/
                (?: # pchar
                    (?:
                        [[:alpha:]
        [:digit:]._ ~-] |
                        (?: % [[:xdigit:]]{2} ) |
                        (?: [!$&'()*+,;=] ) |
                        : |
                        @
                    )
                )*
            )*
        )
        |
        (?: # path-empty
            (?: # pchar
                (?:
                    [[:alpha:]
        [:digit:]._ ~-] |
                    (?: % [[:xdigit:]]{2} ) |
                    (?: [!$&'()*+,;=] ) |
                    : |
                    @
                )
            ){0}
        )
    )
) # hier-part
(?:
    \?
    (?: # query
        (?:
            (?: # pchar
                (?:
                    [[:alpha:]
    [:digit:]._ ~-] |
                    (?: % [[:xdigit:]]{2} ) |
                    (?: [!$&'()*+,;=] ) |
                    : |
                    @
                )
            ) | \/ | \?
        )*
    ) # query
)?
(?:
    \#
    (?: # fragment
        (?:
            (?: # pchar
                (?:
                    [[:alpha:]
[:digit:]._ ~-] |
                    (?: % [[:xdigit:]]{2} ) |
                    (?: [!$&'()*+,;=] ) |
                    : |
                    @
                )
            ) | \/ | \?
        )*
    ) # fragment
)?
$
            ";

        /// <summary>
        /// The regex for validation a URI.
        /// </summary>
        private static Regex ValidUri = new Regex(RegexUriStr, RegexOptions.Compiled);
        /// <summary>
        /// The regex for group matching a well formed URI string.
        /// </summary>
        private static Regex MatchGroupsUri = new Regex(@"(?ix)^(([^:\/?#]+):)?(\/\/([^\/?#]*))?([^?#]*)(\?([^#]*))?(\#(.*))?$");

        /// <summary>
        /// Checks if the given string is a valid URI string.
        /// </summary>
        /// <param name="uri">The string to validate.</param>
        /// <param name="useRegex">Whether to use a regex pattern to match the URI. (False uses the .NET Framework 'Uri.IsWellFormedUriString')</param>
        /// <returns>Whether the string was a valid URI</returns>
        /// <remarks>The regex used for the optional matching is derived exactly from the RFC 3986 definition. (So far little testing has been done to make sure that it fully works, however it compiles and checks properly without errors)</remarks>
        public static bool IsValidUri(string uri, bool useRegex = false)
        {
            if (useRegex)
                return ValidUri.IsMatch(uri);
            else
                return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }
        
        /// <summary>
        /// Checks if the email string is valid.
        /// </summary>
        /// <param name="email">The string to validate.</param>
        /// <returns>Whether the email is valid.</returns>
        public static bool IsValidEmail(string email)
        {
            return false;
        }
    }
}
