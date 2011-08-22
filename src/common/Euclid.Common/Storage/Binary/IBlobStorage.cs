using System;
using Euclid.Common.Logging;

namespace Euclid.Common.Storage.Binary
{
	public interface IBlobStorage : ILoggingSource
	{
		void Configure(IBlobStorageSettings settings);

		void Delete(Uri uri);

		bool Exists(Uri uri);

		IBlob Get(Uri uri);

		Uri Put(IBlob blob, string name);
	}
}