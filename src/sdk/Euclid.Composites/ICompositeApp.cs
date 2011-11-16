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

		 string Name { get; set; }

		CompositeAppSettings Settings { get; }

		CompositeApplicationState State { get; set; }

		void AddAgent(Assembly assembly);

		void Configure(CompositeAppSettings compositeAppSettings);

		void CreateSchema(IPersistenceConfigurer databaseConfiguration, bool destructive);

		//IPartMetadata GetCommandMetadataForInputModel(Type inputModelType);

		IEnumerable<string> GetConfigurationErrors();

		IMetadataFormatter GetFormatter();

		bool IsValid();

		// void RegisterInputModel(IInputToCommandConverter converter);

		void RegisterInputModelMap<TInputModelSource, TCommandDestination>() where TInputModelSource : IInputModel
			where TCommandDestination : ICommand;

		void RegisterInputModelMap<TInputModelSource, TCommandDestination>(
			Func<TInputModelSource, TCommandDestination> customMap) where TInputModelSource : IInputModel
			where TCommandDestination : ICommand;
	}
}