using System;
using System.Collections.Generic;
using System.Reflection;
using Euclid.Common.Logging;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using FluentNHibernate.Cfg.Db;

namespace Euclid.Composites
{
	public interface ICompositeApp : ILoggingSource
	{
		IEnumerable<IAgentMetadata> Agents { get; }

		string Description { get; set; }

		IEnumerable<ITypeMetadata> InputModels { get; }

		IEnumerable<IPartMetadata> Commands { get; }

		IEnumerable<IPartMetadata> Queries { get; }

		string Name { get; set; }

		CompositeAppSettings Settings { get; }

		CompositeApplicationState State { get; set; }

		void AddAgent(Assembly assembly);

		void Configure(CompositeAppSettings compositeAppSettings);

		IPartMetadata GetCommandMetadataForInputModel(Type inputModelType);

		Type GetInputModelTypeForCommandName(string commandName);

		ICommand GetCommandForInputModel(IInputModel model);

		IEnumerable<string> GetConfigurationErrors();

		IMetadataFormatter GetFormatter();

		bool IsValid();

		void RegisterInputModelMap<TInputModelSource, TCommandDestination>()
			where TInputModelSource : IInputModel
			where TCommandDestination : ICommand;

		void RegisterInputModelMap<TInputModelSource, TCommandDestination>(
			Func<TInputModelSource, TCommandDestination> customMap)
			where TInputModelSource : IInputModel
			where TCommandDestination : ICommand;

		void CreateSchema(IPersistenceConfigurer databaseConfiguration, bool destructive);

		object ExecuteQuery(string queryName, string queryMethod, int argumentCount, Func<string, string> getArgumentValue);
	}
}