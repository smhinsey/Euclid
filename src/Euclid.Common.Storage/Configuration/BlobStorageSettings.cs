using Euclid.Common.Configuration;

namespace Euclid.Common.Storage.Configuration
{
	public class BlobStorageSettings : IBlobStorageSettings
	{
		public BlobStorageSettings()
		{
			ContainerName = new OverridableSetting<string>();

			ContainerName.WithDefault("euclidblobstorage");
		}

		public IOverridableSetting<string> ContainerName { get; set; }
	}
}