using System;
using Euclid.Common.Configuration;

namespace Euclid.Common.Storage.Blob
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
