using System.Collections.Generic;

namespace Euclid.Common.Storage
{
    public interface IBlob
    {
        byte[] Bytes { get; set; }
        string ContentType { get; set; }
        IList<KeyValuePair<string, string>> Metdata { get; }
        string MD5 { get; }
        string ETag { get; }
    }
}