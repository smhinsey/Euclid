using System.Collections.Generic;
using System.Reflection;
using Euclid.Common.Logging;
using Euclid.Composites.Conversion;
using Euclid.Framework.AgentMetadata;
using FluentNHibernate.Cfg.Db;

namespace Euclid.Composites
{
	public interface ICompositeApp : ILoggingSource
	{
		IEnumerable<IAgentMetadata> Agents { get; }

		string Description { get; set; }

		IEnumerable<ITypeMetadata> InputModels { get; }

		new string Name { get; set; }

		CompositeAppSettings Settings { get; }

		CompositeApplicationState State { get; set; }

		void AddAgent(Assembly assembly);

		void Configure(CompositeAppSettings compositeAppSettings);

		IPartMetadata GetCommandForInputModel(ITypeMetadata typeMetadata);

		IEnumerable<string> GetConfigurationErrors();

		IMetadataFormatter GetFormatter();

		bool IsValid();

		void RegisterInputModel(IInputToCommandConverter converter);
		void CreateSchema(IPersistenceConfigurer databaseConfiguration);
	}
}