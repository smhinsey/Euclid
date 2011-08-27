using System.IO;
using System.Text;

namespace Euclid.Common.Extensions
{
	public static class StreamExtensions
	{
		public static byte[] GetBytes(this Stream stream)
		{
			var bytes = new byte[stream.Length];

			var pos = stream.Position;
			stream.Seek(0, SeekOrigin.Begin);
			stream.Read(bytes, 0, bytes.Length);
			stream.Seek(pos, SeekOrigin.Begin);

			return bytes;
		}

		public static string GetString(this Stream stream, Encoding encoding)
		{
			var bytes = stream.GetBytes();

			return encoding.GetString(bytes);
		}
	}
}