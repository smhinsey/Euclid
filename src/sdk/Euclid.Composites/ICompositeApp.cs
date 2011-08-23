using System;
using System.Collections.Generic;
using System.Reflection;
using Euclid.Common.Logging;
using Euclid.Composites.Conversion;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.Models;

namespace Euclid.Composites
{
    public interface ICompositeApp : ILoggingSource
	{
		IEnumerable<IAgentMetadata> Agents { get; }
        IEnumerable<ITypeMetadata> InputModels { get; }
    
        CompositeApplicationState State { get; set; }

		void AddAgent(Assembly assembly);

		void Configure(CompositeAppSettings compositeAppSettings);

		void RegisterInputModel(IInputToCommandConverter converter);

	    IMetadataFormatter GetFormatter();
        IEnumerable<string> GetConfigurationErrors();
	    bool IsValid();
	}
}