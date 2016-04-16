using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Genlib.Cryptography
{
    /// <summary>
    /// Provides functions that wrap around the build in encryption of the .NET Framework.
    /// </summary>
    public class Encryption
    {
        private const int DefaultBlockSize = 256;
        private const CipherMode DefaultCipherMode = CipherMode.CBC;
        private const PaddingMode DefaultPaddingMode = PaddingMode.ISO10126;

        private static byte[] SymmetricInternal(byte[] bytes, byte[] key, byte[] iv, bool encrypt, int blockSize, CipherMode cipherMode, PaddingMode paddingMode)
        {
            using (Rijndael rManaged = new RijndaelManaged())
            {
                rManaged.BlockSize = blockSize;
                rManaged.Mode = cipherMode;
                rManaged.Padding = paddingMode;
                Func<byte[], byte[], ICryptoTransform> createFunc;
                CryptoStreamMode cryptmode;
                Func<byte[], MemoryStream> MemCreatorFunc;
                if (encrypt)
                {
                    createFunc = rManaged.CreateEncryptor;
                    cryptmode = CryptoStreamMode.Write;
                    MemCreatorFunc = (membytes) => new MemoryStream();
                }
                else
                {
                    createFunc = rManaged.CreateDecryptor;
                    cryptmode = CryptoStreamMode.Read;
                    MemCreatorFunc = (membytes) => new MemoryStream(membytes);
                }
                using (ICryptoTransform cryptoTransform = createFunc(key, iv))
                {
                    using (MemoryStream memstream = MemCreatorFunc(bytes))
                    {
                        using (CryptoStream cryptstream = new CryptoStream(memstream, cryptoTransform, cryptmode))
                        {
                            byte[] finishedbytes;
                            if (encrypt)
                            {
                                cryptstream.Write(bytes, 0, bytes.Length);
                                cryptstream.FlushFinalBlock();
                                finishedbytes = memstream.ToArray();
                            }
                            else
                            {
                                //finishedbytes = new byte[bytes.Length];
                                //cryptstream.Read(finishedbytes, 0, finishedbytes.Length);
                                using (MemoryStream tmpstream = new MemoryStream())
                                {
                                    cryptstream.CopyTo(tmpstream);
                                    finishedbytes = tmpstream.ToArray();
                                    tmpstream.Close();
                                }
                            }
                            memstream.Close();
                            cryptstream.Close();
                            return finishedbytes;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts a bytearray using the specified key and IV.
        /// </summary>
        /// <param name="bytes">The bytearray to encrypt.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="iv">The IV to use.</param>
        /// <returns>Encrypted bytearray.</returns>
        public static byte[] Encrypt(byte[] bytes, byte[] key, byte[] iv)
        {
            return SymmetricInternal(bytes, key, iv, true, DefaultBlockSize, DefaultCipherMode, DefaultPaddingMode);
        }

        /// <summary>
        /// Encrypts a string using the specified key and IV.
        /// </summary>
        /// <param name="str">The string to encrypt.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="iv">The IV to use.</param>
        /// <returns>Encrypted string.</returns>
        /// <remarks>Uses Encoding.UTF8 to convert the string into bytes.</remarks>
        public static string Encrypt(string str, byte[] key, byte[] iv)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(str), key, iv));
        }

        /// <summary>
        /// Decrypts a bytearray usingt he specified key and IV.
        /// </summary>
        /// <param name="bytes">The bytearray to decrypt.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="iv">The IV to use.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] bytes, byte[] key, byte[] iv)
        {
            return SymmetricInternal(bytes, key, iv, false, DefaultBlockSize, DefaultCipherMode, DefaultPaddingMode);
        }

        /// <summary>
        /// Decrypts a string using the specified key and IV.
        /// </summary>
        /// <param name="str">The string to Decrypt.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="iv">The IV to use.</param>
        /// <returns>Decrypted string.</returns>
        /// <remarks>Uses Encoding.UTF8 to convert the bytes into a string.</remarks>
        public static string Decrypt(string str, byte[] key, byte[] iv)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(str), key, iv));
        }
    }
}
