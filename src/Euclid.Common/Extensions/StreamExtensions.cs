using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}
