using System.Reflection;
using Euclid.Common.Configuration;

namespace Euclid.Framework.Metadata
{
	public interface IExtractorSettings : IOverridableSettings
	{
		IOverridableSettingList<Assembly> AssembliesContainingCommands { get; }
		IOverridableSettingList<Assembly> AssembliesContainingQueries { get; }
	}
}