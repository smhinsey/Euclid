using System;
using System.Linq;
using Euclid.Composites;
using Euclid.Composites.AgentResolution;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.Cqrs;
using Nancy;

namespace CompositeInspector.Module
{
	public class AgentModule : NancyModule
	{
		private readonly ICompositeApp _compositeApp;
		private readonly ICommandRegistry _registry;
		private const string AgentMetadataRoute = "/agent/{agentSystemName}";
		private const string ReadModelMetadataRoute = "/readModel/{agentSystemName}/{readModelName}";
		private const string PublicationRecordRoute = "/publicationRecord/{identifier}";
		private const string BaseRoute = "composite/api";

		public AgentModule(ICompositeApp compositeApp, ICommandRegistry registry)
			: base(BaseRoute)
		{
			_compositeApp = compositeApp;
			_registry = registry;

			Get[string.Empty] = _ => HttpStatusCode.NoContent;

			Get[AgentMetadataRoute] = p => GetAgent((string) p.agentSystemName);

			Get[ReadModelMetadataRoute] = p => GetReadModel((string) p.agentSystemName, (string) p.readModelName);

			Get[PublicationRecordRoute] = p => GetPublicationRecord((Guid) p.identifier);
		}

		public Response GetPublicationRecord(Guid identifier)
		{
			if (this.GetResponseFormat() == ResponseFormat.Html)
			{
				return HttpStatusCode.NoContent;
			}

			var record = _registry.GetPublicationRecord(identifier);
			if (record == null)
			{
				throw new CommandNotFoundInRegistryException(identifier);
			}

			return this.GetResponseFormat() == ResponseFormat.Json ? Response.AsJson(record) : Response.AsXml(record);
		}

		public Response GetAgent(string agentSystemName)
		{
			if (this.GetResponseFormat() == ResponseFormat.Html)
			{
				return HttpStatusCode.NoContent;
			}

			var agent = getAgent(agentSystemName);
			var formatter = agent.GetFormatter(FormatterType.Full);
			return this.GetFormattedMetadata(formatter);
		}

		public Response GetReadModel(string agentSystemName, string readModelName)
		{
			var agent = getAgent(agentSystemName);
			var model =
				agent.ReadModels.Where(r => r.Name.Equals(readModelName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

			if (model == null)
			{
				throw new ReadModelNotFoundExceptin(readModelName);
			}

			var formatter = model.GetFormatter();
			return this.GetFormattedMetadata(formatter);
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