using System.IO;
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
}