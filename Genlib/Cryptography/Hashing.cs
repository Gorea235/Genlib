using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Genlib.Cryptography
{
    /// <summary>
    /// Provides functions that wrap around the build in hashing of the .NET Framework.
    /// </summary>
    public class Hashing
    {
        /// <summary>
        /// Specifies which algorithm to use.
        /// </summary>
        public enum HashAlgorithm
        {
            /// <summary>
            /// The MD5 hashing algorithm (highly unrecommended, due to collision change and insecurities)
            /// </summary>
            MD5,
            /// <summary>
            /// The SH1 hashing algorithm.
            /// </summary>
            SHA1,
            /// <summary>
            /// The SHA256 hashing algorithm.
            /// </summary>
            SHA256,
            /// <summary>
            /// The SHA384 hashing algorithm.
            /// </summary>
            SHA384,
            /// <summary>
            /// The SHA512 hashing algorithm.
            /// </summary>
            SHA512
        }

        /// <summary>
        /// Hashes a bytearray using the specified algorithm.
        /// </summary>
        /// <param name="bytes">The bytes to hash.</param>
        /// <param name="alg">The enum of the algorithm to use.</param>
        /// <returns>Hashed bytearray.</returns>
        public static byte[] Hash(byte[] bytes, HashAlgorithm alg)
        {
            System.Security.Cryptography.HashAlgorithm hashAlg;
            if (alg == HashAlgorithm.MD5)
                hashAlg = MD5.Create();
            else if (alg == HashAlgorithm.SHA1)
                hashAlg = SHA1.Create();
            else if (alg == HashAlgorithm.SHA256)
                hashAlg = SHA256.Create();
            else if (alg == HashAlgorithm.SHA384)
                hashAlg = SHA384.Create();
            else if (alg == HashAlgorithm.SHA512)
                hashAlg = SHA512.Create();
            else
                throw new ArgumentOutOfRangeException(@"Whoops ¯\_(ツ)_/¯");
            return hashAlg.ComputeHash(bytes);
        }

        /// <summary>
        /// Hashes a string using the specified algorithm.
        /// </summary>
        /// <param name="str">The string to hash.</param>
        /// <param name="alg">The enum of the algorithm to use.</param>
        /// <returns>Hashed string.</returns>
        /// <remarks>Uses Encoding.UTF8 to convert the string to a bytearray, and Convert.ToBase64String to convert it back to a string.</remarks>
        public static string Hash(string str, HashAlgorithm alg) { return Convert.ToBase64String(Hash(Encoding.UTF8.GetBytes(str), alg)); }
    }
}
