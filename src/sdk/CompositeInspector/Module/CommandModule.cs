using System;
using System.IO;
using System.Linq;
using System.Text;
using Castle.Windsor;
using CompositeInspector.Models;
using Euclid.Common.Messaging;
using Euclid.Composites;
using Euclid.Composites.Conversion;
using Euclid.Framework.AgentMetadata.Formatters;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;

namespace CompositeInspector.Module
{
	public class CommandModule : NancyModule
	{
		private const string BaseRoute = "composite/commands";

		private const string CommandRoute = "/{agentSystemName}/{commandName}";

		private const string CommandViewPath = "Commands/view-command.cshtml";

		private const string IndexRoute = "";

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

			Get[IndexRoute] = _ => "Command API";

			Get[CommandRoute] = p =>
				{
					var agentSystemName = (string)p.agentSystemName;
					var commandName = (string)p.commandName;

					return GetCommand(agentSystemName, commandName);
				};

			Post[PublishRoute] = p =>
				{
					var inputModel = this.Bind<IInputModel>();
					if (inputModel == null)
					{
						throw new InvalidOperationException("Unable to retrieve input model from form");
					}

					return PublishCommand(inputModel);
				};

			Get[PublicationStatusRoute] = p => GetCommandStatus((Guid)p.publicationId);
		}

		public Response GetCommand(string agentSystemName, string commandName)
		{
			var asJson = false;

			// TODO: how can we eliminate this sort of thing?
			if (commandName.EndsWith(".json"))
			{
				asJson = true;
				commandName = commandName.Substring(0, commandName.Length - 5);
			}

			var inputModel = getInputModel(agentSystemName, commandName);

			if (inputModel == null)
			{
				return string.Format(
					"The {0} composite application does not support the command '{1}'", _compositeApp.Name, commandName);
			}

			if (asJson)
			{
				var f = new InputModelFormatter(inputModel);
				var s = new MemoryStream(Encoding.UTF8.GetBytes(f.GetAsJson()));
				return Response.FromStream(s, "application/json");
			}

			var commandModel = new CommandModel { AgentSystemName = agentSystemName, CommandName = commandName };

			return View[CommandViewPath, commandModel];
		}

		public Response GetCommandStatus(Guid publicationId)
		{
			Exception registryException = null;
			ICommandPublicationRecord record = null;

			try
			{
				record = _registry.GetPublicationRecord(publicationId);

				if (record == null)
				{
					throw new CommandNotFoundInRegistryException(publicationId);
				}
			}
			catch (Exception e)
			{
				registryException = e;
			}

			if (Request.IsAjaxRequest())
			{
				if (registryException == null)
				{
					return Response.AsJson(record);
				}

				var exceptionModel =
					new
						{
							name = registryException.GetType().Name,
							message = registryException.Message,
							callStack = registryException.StackTrace
						};

				return Response.AsJson(exceptionModel, HttpStatusCode.InternalServerError);
			}

			var model = new PublishedCommandModel { PublicationId = publicationId.ToString(), Record = record };
			return View["Commands/view-publication-record.cshtml", model];
		}

		public Response PublishCommand(IInputModel inputModel)
		{
			Exception publishingException = null;

			Guid publicationId;
			try
			{
				var command = _compositeApp.GetCommandForInputModel(inputModel);

				publicationId = _publisher.PublishMessage(command);
			}
			catch (Exception e)
			{
				publishingException = e;
				publicationId = Guid.Empty;
			}

			// TODO: how can we eliminate this sort of thing?
			if (Request.IsAjaxRequest())
			{
				if (publishingException == null)
				{
					return Response.AsJson(new { publicationId });
				}

				var exceptionModel =
					new
						{
							name = publishingException.GetType().Name,
							message = publishingException.Message,
							callStack = publishingException.StackTrace
						};

				return Response.AsJson(exceptionModel, HttpStatusCode.InternalServerError);
			}

			string redirectUrl;

			var incomingAlternateRedirectUrl = Request.Form["alternateRedirectUrl"].Value;

			if (!string.IsNullOrEmpty(incomingAlternateRedirectUrl))
			{
				redirectUrl = (string)incomingAlternateRedirectUrl;
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

	public class CommandNotFoundInRegistryException : Exception
	{
		public CommandNotFoundInRegistryException(Guid publicationId) : base(publicationId.ToString())
		{
		}
	}
}