using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Common.Configuration;

namespace Euclid.Framework.Cqrs.Metadata
{
    public class MetadataServiceSettings : IExtractorSettings
    {
        private readonly IOverridableSettingList<Assembly> _assemblies;

        public MetadataServiceSettings()
        {
            _assemblies = new OverridableSettingList<Assembly>();
        }

        public MetadataServiceSettings(IEnumerable<Assembly> assemblies)
        {
            _assemblies = new OverridableSettingList<Assembly>();
            assemblies.ToList().ForEach(_assemblies.Add);
        }

        public IOverridableSettingList<Assembly> AssembliesContainingCommands
        {
            get { return _assemblies; }
        }
    }
}