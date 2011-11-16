using System.Security.Cryptography;
using System.Text;

namespace Euclid.Common.Extensions
{
	public static class StringExtensions
	{
		public static string GetMd5(this string input)
		{
			// http://blogs.msdn.com/b/csharpfaq/archive/2006/10/09/how-do-i-calculate-a-md5-hash-from-a-string_3f00_.aspx

			// step 1, calculate MD5 hash from input
			var md5 = MD5.Create();
			var inputBytes = Encoding.ASCII.GetBytes(input);
			var hash = md5.ComputeHash(inputBytes);

			// step 2, convert byte array to hex string
			var sb = new StringBuilder();
			for (var i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString().ToLowerInvariant();
		}
	}
}