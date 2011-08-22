using Euclid.Common.Configuration;
using Euclid.Common.Storage.Binary;

namespace Euclid.Common.Storage
{
	public class BlobStorageSettings : IBlobStorageSettings
	{
		public BlobStorageSettings()
		{
			this.ContainerName = new OverridableSetting<string>();

			this.ContainerName.WithDefault("euclidblobstorage");
		}

		public IOverridableSetting<string> ContainerName { get; set; }
	}
}