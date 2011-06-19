using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Euclid.Common.Extensions
{
    public static class StringExtensions
    {
        public static Stream ToMemoryStream(this string s, Encoding encoding)
        {
            var bytes = encoding.GetBytes(s);

            return new MemoryStream(bytes);
        }
    }

    public static class ByteArrayExtensions
    {
        public static string GetMd5Hash(this byte[] bytes)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            var md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(bytes);

            // Return the hexadecimal string.
            return Convert.ToBase64String(data);
        }

        public static string GetString(this byte[] bytes, Encoding encoding)
        {
            var stringValue = string.Empty;

            using (var stream = new MemoryStream(bytes))
            {
                stringValue = stream.GetString(encoding);
            }

            return stringValue;
        }
    }
}