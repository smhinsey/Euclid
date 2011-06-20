using Euclid.Common.Configuration;

namespace Euclid.Common.Storage
{
	public interface IBlobStorageSettings : IOverridableSettings
	{
		IOverridableSetting<string> ContainerName { get; set; }
	}
}