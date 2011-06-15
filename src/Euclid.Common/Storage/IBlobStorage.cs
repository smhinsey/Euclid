using System;
using System.Collections.Generic;
using System.IO;

namespace Euclid.Common.Storage
{
	public interface IBlobStorage
	{
		void Delete(string uri);
		void Delete(Uri uri);

		bool Exists(Uri uri);
		bool Exists(string uri);

		byte[] Get(Uri uri);
		byte[] Get(string uri);

		Uri Put(Stream blob, string name, IList<KeyValuePair<string, string>> metadata);
	}
}