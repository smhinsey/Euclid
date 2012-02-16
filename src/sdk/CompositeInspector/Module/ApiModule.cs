using System;
using System.IO;
using System.Linq;
using CompositeInspector.Extensions;
using Euclid.Common.Messaging;
using Euclid.Composites;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.ActionFilters;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using LoggingAgent.Queries;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace CompositeInspector.Module
{
	public class ApiModule : NancyModule
	{
		private readonly ICompositeApp _compositeApp;
		private readonly ICommandRegistry _registry;
		private readonly IPublisher _publisher;

		private const string AgentMetadataRoute                = "/agent/{agentSystemName}";
		private const string ReadModelMetadataRoute            = "/readModel/{agentSystemName}/{readModelName}";
		private const string InputModelMetadataRoute           = "/inputModel/{inputModelName}";
		private const string PublicationRecordRoute            = "/publicationRecord/{identifier}";
		private const string InputModelMetadataForCommandRoute = "/command/{commandName}";
		private const string CommandMetadataRoute              = "/command-metadata/{commandName}";
		private const string QueryMetadataRoute                = "/query-metadata/{queryName}";
		private const string ExecuteQueryRoute                 = "/execute/query/{queryName}/{methodName}";
		private const string PublishCommandRoute               = "/publish";
		private const string BaseRoute                         = "composite/api";

		public ApiModule(ICompositeApp compositeApp, ICommandRegistry registry, IPublisher publisher)
			: base(BaseRoute)
		{
			_compositeApp = compositeApp;
			_registry = registry;
			_publisher = publisher;

			Get[string.Empty] = _ => GetComposite();

			Get[AgentMetadataRoute] = p => GetAgent(stripExtension((string) p.agentSystemName));

			Get[ReadModelMetadataRoute] = p => GetReadModel((string) p.agentSystemName, stripExtension((string) p.readModelName));

			Get[InputModelMetadataRoute] = p => GetInputModel(stripExtension((string) p.inputModelName));

			Get[PublicationRecordRoute] = p => GetPublicationRecord((Guid) p.identifier);

			Get[InputModelMetadataForCommandRoute] = p => GetInputModelMetadata(stripExtension((string) p.commandName));

			Get[CommandMetadataRoute] = p => GetCommandMetadata(stripExtension((string) p.commandName));

			Get[QueryMetadataRoute] = p => GetQueryMetadata(stripExtension((string) p.queryName));

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

			var redirectUrl = Context.Request.Form["redirectUrl"].Value;

			return !string.IsNullOrEmpty(redirectUrl)
						? new RedirectResponse(redirectUrl)
						: GetPublicationRecord(publicationId);
		}

		public Response ExecuteQuery(string queryName, string methodName)
		{
			var form = (DynamicDictionary)Context.Request.Form;
			var argumentCount = form.Count();
			var results = _compositeApp.ExecuteQuery(queryName, methodName, argumentCount, paramName => form[paramName]);

			return formatReturnData(results);
		}
		
		public Response GetCommandMetadata(string commandName)
		{
			var agentCommands = _compositeApp.Agents.SelectMany(x => x.Commands);

			var command = agentCommands.FirstOrDefault(c => c.Name.Equals(commandName, StringComparison.CurrentCultureIgnoreCase));

			if (command == null)
			{
				throw new CommandNotFoundInAgentException(commandName);
			}

			return command.GetFormatter().WriteTo(Response);
		}

		public Response GetInputModelMetadata(string commandName)
		{
			try
			{
				var inputModelType = _compositeApp.GetInputModelTypeForCommandName(commandName);

				return inputModelType.GetMetadata().GetFormatter().WriteTo(Response);
			}
			catch (CannotMapCommandException)
			{
				throw new CommandNotFoundInCompositeException(commandName);
			}
		}

		public Response GetInputModel(string inputModelName)
		{
			var inputModel =
				_compositeApp.InputModels.FirstOrDefault(x => x.Name.Equals(inputModelName, StringComparison.CurrentCultureIgnoreCase));

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

			return formatReturnData(record);
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
			var model = agent.ReadModels.FirstOrDefault(r => r.Name.Equals(readModelName, StringComparison.CurrentCultureIgnoreCase));

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

			if (query == null)
			{
				throw new QueryNotFoundInCompositeException(queryName);
			}

			return query.GetFormatter().WriteTo(Response);
		}

		private IAgentMetadata getAgent(string agentSystemName)
		{
			var agent =
				_compositeApp.Agents.FirstOrDefault(a => a.SystemName.Equals(agentSystemName, StringComparison.CurrentCultureIgnoreCase));

			if (agent == null)
			{
				throw new AgentNotFoundException(agentSystemName);
			}

			return agent;
		}

		private Response formatReturnData(object data)
		{
			Response response;
			var format = Context.GetResponseFormat();
			switch (format)
			{
				case ResponseFormat.Json:
					response = Response.AsJson(data);
					break;
				case ResponseFormat.Xml:
					response = Response.AsXml(data);
					break;
				default:
					response = HttpStatusCode.NoContent;
					break;
			}

			return response;
		}

		private string stripExtension(string name)
		{
			var format = this.GetResponseFormat();
			if ((format == ResponseFormat.Json || format == ResponseFormat.Xml) && (name.EndsWith(".json") || name.EndsWith(".xml")))
			{
				var pos = name.LastIndexOf('.');
				return pos >= 0 ? name.Substring(0, pos) : name;
			}

			return name;
		}
	}
}