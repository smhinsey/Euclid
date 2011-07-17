using System.Reflection;
using Euclid.Common.Configuration;

namespace Euclid.Framework.Cqrs.Metadata
{
	public interface IExtractorSettings : IOverridableSettings
	{
		IOverridableSettingList<Assembly> AssembliesContainingCommands { get; }
		IOverridableSettingList<Assembly> AssembliesContainingQueries { get; }
	}
}