using System.Collections.Generic;
using System.Reflection;
using Euclid.Common.Logging;
using Euclid.Composites.Conversion;
using Euclid.Framework.AgentMetadata;

namespace Euclid.Composites
{
	public interface ICompositeApp : ILoggingSource
	{
		IEnumerable<IAgentMetadata> Agents { get; }

		string Description { get; set; }

		IEnumerable<ITypeMetadata> InputModels { get; }

		string Name { get; set; }

		CompositeAppSettings Settings { get; }

		CompositeApplicationState State { get; set; }

		void AddAgent(Assembly assembly);

		void Configure(CompositeAppSettings compositeAppSettings);

		IEnumerable<string> GetConfigurationErrors();

		IMetadataFormatter GetFormatter();

		bool IsValid();

		void RegisterInputModel(IInputToCommandConverter converter);

        IPartMetadata GetCommandForInputModel(ITypeMetadata typeMetadata);
	}
}