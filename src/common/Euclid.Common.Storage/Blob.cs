using System;
using System.Collections.Generic;
using Euclid.Common.Extensions;

namespace Euclid.Common.Storage
{
	public class Blob : IBlob
	{
		private byte[] _bytes;

		private string _md5;

		public Blob()
		{
			this.Metdata = new List<KeyValuePair<string, string>>();
		}

		public Blob(string md5, string eTag)
			: this()
		{
			this._md5 = md5;
			this.ETag = eTag;
		}

		public Blob(IBlob blob)
			: this(blob.MD5, Guid.NewGuid().ToString())
		{
			this.Metdata = new List<KeyValuePair<string, string>>(blob.Metdata);
			this.Bytes = blob.Bytes;
			this.ContentType = blob.ContentType;
		}

		public byte[] Bytes
		{
			get
			{
				return this._bytes;
			}

			set
			{
				this._bytes = value;
				this._md5 = this._bytes.GetMd5Hash();
			}
		}

		public string ContentType { get; set; }

		public string ETag { get; private set; }

		public string MD5
		{
			get
			{
				return this._md5;
			}

			set
			{
				this._md5 = value;
			}
		}

		public IList<KeyValuePair<string, string>> Metdata { get; private set; }
	}
}