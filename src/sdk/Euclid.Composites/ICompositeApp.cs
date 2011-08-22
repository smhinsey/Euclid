using System;
using System.Collections.Generic;
using System.Reflection;
using Euclid.Common.Logging;
using Euclid.Composites.Conversion;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.Models;

namespace Euclid.Composites
{
    [Flags]
    public enum CompositeMetadata
    {
        All = 0x0,
        Agents = 0x1,
        InputModels = 0x2,
        Configuration = 0x4,
        ConfigurationErrors = 0x8
    }

    public interface ICompositeApp : ILoggingSource
	{
		IEnumerable<IAgentMetadata> Agents { get; }
        IEnumerable<ITypeMetadata> InputModels { get; }
    
        CompositeApplicationState State { get; set; }
		void AddAgent(Assembly assembly);
		void Configure(CompositeAppSettings compositeAppSettings);
		void RegisterInputModel(IInputToCommandConverter converter);

	    IMetadataFormatter GetFormatter(CompositeMetadata forMetadata);
        IEnumerable<string> GetConfigurationErrors();
	    bool HasConfigurationErrors();
	}
}