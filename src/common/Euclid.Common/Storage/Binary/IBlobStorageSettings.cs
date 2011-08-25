using Euclid.Common.Configuration;

namespace Euclid.Common.Storage.Binary
{
	/// <summary>
	/// 	Settings required to initialize a blob storage engine.
	/// </summary>
	public interface IBlobStorageSettings : IOverridableSettings
	{
		// SELF this seems azure-specific

		/// <summary>
		/// 	Gets or sets the name of the storage container.
		/// </summary>
		IOverridableSetting<string> ContainerName { get; set; }
	}
}
