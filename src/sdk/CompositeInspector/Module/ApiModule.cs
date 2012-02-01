using System;
using System.IO;
using System.Linq;
using Euclid.Common.Messaging;
using Euclid.Composites;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.ActionFilters;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using Nancy;
using Nancy.ModelBinding;

namespace CompositeInspector.Module
{
	public class ApiModule : NancyModule
	{
		private readonly ICompositeApp _compositeApp;
		private readonly ICommandRegistry _registry;
		private readonly IPublisher _publisher;
		private const string AgentMetadataRoute        = "/agent/{agentSystemName}";
		private const string ReadModelMetadataRoute    = "/readModel/{agentSystemName}/{readModelName}";
		private const string InputModelMetadataRoute   = "/inputModel/{inputModelName}";
		private const string PublicationRecordRoute    = "/publicationRecord/{identifier}";
		private const string InputModelForCommandRoute = "/command/{commandName}";
		private const string CommandMetadataRoute      = "/command-metadata/{commandName}";
		private const string QueryMetadataRoute        = "/query-metadata/{queryName}";
		private const string ExecuteQueryRoute         = "/execute/query/{queryName}/{methodName}";
		private const string PublishCommandRoute       = "/publish";
		private const string BaseRoute                 = "composite/api";

		public ApiModule(ICompositeApp compositeApp, ICommandRegistry registry, IPublisher publisher)
			: base(BaseRoute)
		{
			_compositeApp = compositeApp;
			_registry = registry;
			_publisher = publisher;

			Get[string.Empty] = _ => GetComposite();

			Get[AgentMetadataRoute] = p => GetAgent((string) p.agentSystemName);

			Get[ReadModelMetadataRoute] = p => GetReadModel((string) p.agentSystemName, (string) p.readModelName);

			Get[InputModelMetadataRoute] = p => GetInputModel((string) p.inputModelName);

			Get[PublicationRecordRoute] = p => GetPublicationRecord((Guid) p.identifier);

			Get[InputModelForCommandRoute] = p => GetInputModelForCommand((string) p.commandName);

			Get[CommandMetadataRoute] = p => GetCommandMetadata((string) p.commandName);

			Get[QueryMetadataRoute] = p => GetQueryMetadata((string) p.queryName);

			Post[ExecuteQueryRoute] = p => ExecuteQuery((string) p.queryName, (string) p.methodName);

			Post[PublishCommandRoute] = p => PublishCommand(this.Bind<IInputModel>());
		}

		public Response PublishCommand(IInputModel inputModel)
		{
			if (inputModel == null)
			{
				throw new InvalidOperationException("Input Model was not posted");
			}

			var command = _compositeApp.GetCommandForInputModel(inputModel);

			var publicationId = _publisher.PublishMessage(command);

			return GetPublicationRecord(publicationId);
		}

		public Response ExecuteQuery(string queryName, string methodName)
		{
			var form = (DynamicDictionary)Context.Request.Form;
			var argumentCount = form.Count();
			var results = _compositeApp.ExecuteQuery(queryName, methodName, argumentCount, paramName => form[paramName]);

			return Response.AsJson(results);
		}
		
		public Response GetCommandMetadata(string commandName)
		{
			var agentCommands = _compositeApp.Agents.SelectMany(x => x.Commands);

			var command =
				agentCommands.Where(c => c.Name.Equals(commandName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

			if (command == null)
			{
				throw new CommandNotPresentInAgentException(commandName);
			}

			return command.GetFormatter().WriteTo(Response);
		}

		public Response GetInputModelForCommand(string commandName)
		{
			try
			{
				if (commandName.EndsWith(".json"))
				{
					commandName = commandName.Substring(0, commandName.LastIndexOf("."));
				}

				var inputModelType = _compositeApp.GetInputModelTypeForCommandName(commandName);

				return inputModelType.GetMetadata().GetFormatter().WriteTo(Response);
			}
			catch (CannotMapCommandException)
			{
				throw new CommandNotPresentInCompositeException(commandName);
			}
		}

		public Response GetInputModel(string inputModelName)
		{
			var inputModel =
				_compositeApp.InputModels.Where(x => x.Name.Equals(inputModelName, StringComparison.CurrentCultureIgnoreCase)).
					FirstOrDefault();

			if (inputModel == null)
			{
				throw new CannotRetrieveInputModelException(inputModelName);
			}

			return inputModel.GetFormatter().WriteTo(Response);
		}

		public Response GetPublicationRecord(Guid identifier)
		{
			var record = _registry.GetPublicationRecord(identifier);
			if (record == null)
			{
				throw new CommandNotFoundInRegistryException(identifier);
			}

			return this.GetResponseFormat() == ResponseFormat.Json ? Response.AsJson(record) : Response.AsXml(record);
		}

		public Response GetAgent(string agentSystemName)
		{
			var agent = getAgent(agentSystemName);
			if (agent == null)
			{
				throw new AgentNotFoundException(agentSystemName);
			}

			return agent.GetFormatter(FormatterType.Full).WriteTo(Response);
		}

		public Response GetReadModel(string agentSystemName, string readModelName)
		{
			var agent = getAgent(agentSystemName);
			var model = agent.ReadModels.Where(r => r.Name.Equals(readModelName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

			if (model == null)
			{
				throw new ReadModelNotFoundExceptin(readModelName);
			}

			return model.GetFormatter().WriteTo(Response);
		}

		public Response GetComposite()
		{
			return _compositeApp.GetFormatter().WriteTo(Response);
		}

		public Response GetQueryMetadata(string queryName)
		{
			var queries = _compositeApp.Queries;

			var query = queries.FirstOrDefault(q => q.Name.Equals(queryName, StringComparison.InvariantCultureIgnoreCase));

			return query.GetFormatter().WriteTo(Response);
		}


		private IAgentMetadata getAgent(string agentSystemName)
		{
			var agent =
				_compositeApp.Agents.Where(a => a.SystemName.Equals(agentSystemName, StringComparison.CurrentCultureIgnoreCase)).
					FirstOrDefault();

			if (agent == null)
			{
				throw new AgentNotFoundException(agentSystemName);
			}

			return agent;
		}
	}
}