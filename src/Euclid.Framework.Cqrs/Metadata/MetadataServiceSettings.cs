using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Common.Configuration;

namespace Euclid.Framework.Cqrs.Metadata
{
	public class MetadataServiceSettings : IExtractorSettings
	{
		private readonly IOverridableSettingList<Assembly> _commandAssemblies;
		private readonly IOverridableSettingList<Assembly> _queryAssemblies;

		public MetadataServiceSettings()
		{
			_commandAssemblies = new OverridableSettingList<Assembly>();
			_commandAssemblies.WithDefault(new List<Assembly>());

			_queryAssemblies = new OverridableSettingList<Assembly>();
			_queryAssemblies.WithDefault(new List<Assembly>());
		}

		public MetadataServiceSettings(IEnumerable<Assembly> assemblies) : this()
		{
			assemblies.ToList().ForEach(_commandAssemblies.Add);
		}

		public IOverridableSettingList<Assembly> AssembliesContainingCommands
		{
			get { return _commandAssemblies; }
		}

		public IOverridableSettingList<Assembly> AssembliesContainingQueries
		{
			get { return _queryAssemblies; }
		}
	}
}