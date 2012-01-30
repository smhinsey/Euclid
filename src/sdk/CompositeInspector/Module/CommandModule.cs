using System;
using System.Linq;
using Castle.Windsor;
using CompositeInspector.Models;
using Euclid.Common.Messaging;
using Euclid.Composites;
using Euclid.Composites.Conversion;
using Euclid.Framework.AgentMetadata.Formatters;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using Nancy;
using Nancy.ModelBinding;

namespace CompositeInspector.Module
{
	public class CommandModule : NancyModule
	{
		private const string IndexRoute = "";

		private const string BaseRoute = "composite/commands";

		private const string CommandRoute = "/{agentSystemName}/{commandName}";

		// NOTE: work around until Nancy is fixed to handle the route assignments a bit better -  we are looking for /{agentSystemName}/{commandName}.{extension}
		private const string CommandRouteWithFormat =
			"/{agentSystemName}/(?<commandName>[A-Za-z0-9]*)\\.(?<extension>[A-Za-z0-9]*)";

		private const string CommandViewPath = "Commands/view-command.cshtml";

		private const string PublicationStatusRoute = "/status/{publicationId}";

		private const string PublishRoute = "/publish";

		private readonly ICompositeApp _compositeApp;

		private readonly IWindsorContainer _container;

		private readonly IPublisher _publisher;

		private readonly ICommandRegistry _registry;

		public CommandModule(IWindsorContainer container)
			: base(BaseRoute)
		{
			_container = container;
			_compositeApp = _container.Resolve<ICompositeApp>();
			_publisher = _container.Resolve<IPublisher>();
			_registry = _container.Resolve<ICommandRegistry>();

			Get[IndexRoute] = _ => HttpStatusCode.NoContent;

			Get[CommandRouteWithFormat] = p => GetCommand((string) p.agentSystemName, (string) p.commandName);

			Get[CommandRoute] = p => GetCommand((string) p.agentSystemName, (string) p.commandName);

			Post[PublishRoute] = p => PublishCommand(this.Bind<IInputModel>());

			Get[PublicationStatusRoute] = p => GetCommandStatus((Guid) p.publicationId);
		}

		public Response GetCommand(string agentSystemName, string commandName)
		{
			var inputModel = getInputModel(agentSystemName, commandName);

			if (inputModel == null)
			{
				throw new CommandNotPresentInAgentException(commandName);
			}

			var format = this.GetResponseFormat();

			if (format == ResponseFormat.Html)
			{
				var commandModel = new CommandModel {AgentSystemName = agentSystemName, CommandName = commandName};
				return View[CommandViewPath, commandModel];
			}

			return this.GetFormattedMetadata(new InputModelFormatter(inputModel));
		}

		public Response GetCommandStatus(Guid publicationId)
		{
			ICommandPublicationRecord record = null;

			record = _registry.GetPublicationRecord(publicationId);

			if (record == null)
			{
				throw new CommandNotFoundInRegistryException(publicationId);
			}

			var format = this.GetResponseFormat();
			Response response;
			if (format == ResponseFormat.Html)
			{
				var model = new PublishedCommandModel {PublicationId = publicationId.ToString(), Record = record};
				response = View["Commands/view-publication-record.cshtml", model];
			}
			else if (format == ResponseFormat.Json)
			{
				response = Response.AsJson(record);
			}
			else
			{
				response = Response.AsXml(record);
			}

			return response;
		}

		public Response PublishCommand(IInputModel inputModel)
		{
			if (inputModel == null)
			{
				throw new InvalidOperationException("Unable to retrieve input model from form");
			}

			var command = _compositeApp.GetCommandForInputModel(inputModel);

			var publicationId = _publisher.PublishMessage(command);

			var format = this.GetResponseFormat();

			if (format == ResponseFormat.Html)
			{
				string redirectUrl;

				var incomingAlternateRedirectUrl = Request.Form["alternateRedirectUrl"].Value;

				if (!string.IsNullOrEmpty(incomingAlternateRedirectUrl))
				{
					redirectUrl = (string) incomingAlternateRedirectUrl;
				}
				else
				{
					// TODO: i find myself missing mvc's routing engine, surprisingly.
					var publicationStatusUrl = string.Format("/commands/status/{0}", publicationId);

					var referringUrl = Request.Headers["REFERER"].FirstOrDefault();

					redirectUrl = referringUrl ?? publicationStatusUrl;
				}

				return Response.AsRedirect(redirectUrl);
			}

			return GetCommandStatus(publicationId);
		}

		private IInputModel getInputModel(string agentSystemName, string commandName)
		{
			var agents = _compositeApp.Agents;

			var agent =
				agents.FirstOrDefault(a => a.SystemName.Equals(agentSystemName, StringComparison.InvariantCultureIgnoreCase));

			if (agent == null)
			{
				return null;
			}

			var commands = agent.Commands;

			var command = commands.FirstOrDefault(c => c.Name.Equals(commandName, StringComparison.InvariantCultureIgnoreCase));

			if (command == null)
			{
				return null;
			}

			try
			{
				var inputModelType = _compositeApp.GetInputModelTypeForCommandName(command.Name);

				return _container.Resolve<IInputModel>(inputModelType.Name);
			}
			catch (CannotMapCommandException)
			{
				return null;
			}
		}
	}
}