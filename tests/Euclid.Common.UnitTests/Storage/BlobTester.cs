using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Euclid.Common.Configuration;
using Euclid.Common.Logging;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Blob;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Storage
{
    public class BlobTester
    {
        private readonly IBlobStorage _blobStorage;

        public BlobTester(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public IBlob GetNewBlob()
        {
            return new Blob
                       {
                           Bytes =
                               Encoding.UTF8.GetBytes(
                                   string.Format(
                                       "<blob><title>Test Blob</title><created>{0}</created><testing>{1}</testing></blob>",
                                       DateTime.Now, _blobStorage.GetType().FullName)),
                           ContentType = "text/xml",
                       };
        }

        public Uri Put(IBlob blob)
        {
            Assert.NotNull(blob);
            Assert.NotNull(blob.Bytes);

            var uri = _blobStorage.Put(blob, "test");

            Assert.NotNull(uri);

            return uri;
        }

        public Uri Delete(Uri uri)
        {
            Assert.NotNull(uri);
            Assert.True(_blobStorage.Exists(uri));

            _blobStorage.Delete(uri);
            Assert.False(_blobStorage.Exists(uri));

            return uri;
        }

        public IBlob Get(Uri uri)
        {
            Assert.NotNull(uri);

            var retrieved = _blobStorage.Get(uri);
            return retrieved;
        }

        public bool Exists(Uri uri)
        {
            Assert.NotNull(uri);
            return _blobStorage.Exists(uri);
        }
    }
}
