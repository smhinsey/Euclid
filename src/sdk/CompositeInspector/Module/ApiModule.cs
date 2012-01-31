using System;
using System.Linq;
using Euclid.Composites;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.ActionFilters;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Framework.Cqrs;
using Nancy;

namespace CompositeInspector.Module
{
	public class ApiModule : NancyModule
	{
		private readonly ICompositeApp _compositeApp;
		private readonly ICommandRegistry _registry;
		private const string AgentMetadataRoute = "/agent/{agentSystemName}";
		private const string ReadModelMetadataRoute = "/readModel/{agentSystemName}/{readModelName}";
		private const string InputModelMetadataRoute = "/inputModel/{inputModelName}";
		private const string PublicationRecordRoute = "/publicationRecord/{identifier}";
		private const string CommandMetadataRoute = "/command/{commandName}";

		private const string BaseRoute = "composite/api";

		public ApiModule(ICompositeApp compositeApp, ICommandRegistry registry)
			: base(BaseRoute)
		{
			_compositeApp = compositeApp;
			_registry = registry;

			Get[string.Empty] = _ => GetComposite();

			Get[AgentMetadataRoute] = p => GetAgent((string) p.agentSystemName);

			Get[ReadModelMetadataRoute] = p => GetReadModel((string) p.agentSystemName, (string) p.readModelName);

			Get[InputModelMetadataRoute] = p => GetInputModel((string) p.inputModelName);

			Get[PublicationRecordRoute] = p => GetPublicationRecord((Guid) p.identifier);

			Get[CommandMetadataRoute] = p => GetCommandMetadata((string) p.commandName);
		}

		public Response GetCommandMetadata(string commandName)
		{
			try
			{
				var inputModelType = _compositeApp.GetInputModelTypeForCommandName(commandName);

				return inputModelType.GetMetadata().GetFormatter().WriteTo(Response);
			}
			catch (CannotMapCommandException)
			{
				throw new CommandNotPresentInAgentException(commandName);
			}
		}

		public Response GetInputModel(string inputModelName)
		{
			var inputModel =
				_compositeApp.InputModels.Where(x => x.Name.Equals(inputModelName, StringComparison.CurrentCultureIgnoreCase)).
					FirstOrDefault();

			if (inputModel == null)
			{
				throw new CannotRetrieveInputModelException();
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

	public class ReadModelNotFoundExceptin : Exception
	{
		public ReadModelNotFoundExceptin(string readModelName) : base(readModelName)
		{
		}
	}
}