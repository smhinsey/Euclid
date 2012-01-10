using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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

		public static string RemoveAccent(this string txt)
		{
			var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
			return Encoding.ASCII.GetString(bytes);
		}

		public static string Slugify(this string phrase)
		{
			var str = phrase.RemoveAccent().ToLower();

			str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars           
			str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space   
			str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); // cut and trim it   
			str = Regex.Replace(str, @"\s", "-"); // hyphens   

			return str;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="wordsAndTheirReplacements">A dictionary containing the words to locate as keys and their replacements as values.</param>
		/// <returns></returns>
		public static string Censor(this string text, IDictionary<string,string> wordsAndTheirReplacements)
		{
			foreach (var wordAndReplacement in wordsAndTheirReplacements)
			{
				text = Regex.Replace(text, wordAndReplacement.Key, wordAndReplacement.Value,
				RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
			}

			return text;
		}
	}
}