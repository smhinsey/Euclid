using Euclid.Common.Configuration;

namespace Euclid.Common.Storage.Binary
{
	public interface IBlobStorageSettings : IOverridableSettings
	{
		IOverridableSetting<string> ContainerName { get; set; }
	}
}