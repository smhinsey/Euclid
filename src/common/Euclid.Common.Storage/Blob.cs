using System;
using System.Collections.Generic;
using Euclid.Common.Extensions;

namespace Euclid.Common.Storage
{
	public class Blob : IBlob
	{
		private byte[] _content;

		private string _md5;

		public Blob()
		{
			Metdata = new List<KeyValuePair<string, string>>();
		}

		public Blob(string md5, string eTag)
			: this()
		{
			_md5 = md5;
			ETag = eTag;
		}

		public Blob(IBlob blob)
			: this(blob.Md5, Guid.NewGuid().ToString())
		{
			Metdata = new List<KeyValuePair<string, string>>(blob.Metdata);
			Content = blob.Content;
			ContentType = blob.ContentType;
		}

		public byte[] Content
		{
			get { return _content; }

			set
			{
				_content = value;
				_md5 = _content.GetMd5Hash();
			}
		}

		public string ContentType { get; set; }

		public string ETag { get; private set; }

		public string Md5
		{
			get { return _md5; }

			set { _md5 = value; }
		}

		public IList<KeyValuePair<string, string>> Metdata { get; private set; }
	}
}
