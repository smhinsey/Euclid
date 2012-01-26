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
		private readonly ICompositeApp _compositeApp;

		private readonly IWindsorContainer _container;

		private readonly IPublisher _publisher;

		private readonly ICommandRegistry _registry;

		public CommandModule(IWindsorContainer container)
			: base("composite/commands")
		{
			_container = container;
			_compositeApp = _container.Resolve<ICompositeApp>();
			_publisher = _container.Resolve<IPublisher>();
			_registry = _container.Resolve<ICommandRegistry>();

			Get[""] = _ => "Command API";

			Get["/{agentSystemName}/{commandName}"] = p =>
				{
					var agentSystemName = (string)p.agentSystemName;
					var commandName = (string)p.commandName;

					return GetCommand(agentSystemName, commandName);
				};

			Post["/publish"] = p =>
				{
					var inputModel = this.Bind<IInputModel>();
					if (inputModel == null)
					{
						throw new InvalidOperationException("Unable to retrieve input model from form");
					}

					return ExecuteCommand(inputModel);
				};

			Get["/status/{publicationId}"] = p => { return GetCommandStatus((Guid)p.publicationId); };
		}

		public Response ExecuteCommand(IInputModel inputModel)
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

			if (Request.IsAjaxRequest())
			{
				return (publishingException == null)
				       	? Response.AsJson(new { publicationId })
				       	: Response.AsJson(
				       		new
				       			{
				       				name = publishingException.GetType().Name,
				       				message = publishingException.Message,
				       				callStack = publishingException.StackTrace
				       			},
				       		HttpStatusCode.InternalServerError);
			}

			var redirectUrl = !string.IsNullOrEmpty(Request.Form["alternateRedirectUrl"].Value)
			                  	? (string)Request.Form["alternateRedirectUrl"].Value
			                  	: Request.Headers["REFERER"].FirstOrDefault()
			                  	  ?? string.Format("/commands/status/{0}", publicationId);

			return Response.AsRedirect(redirectUrl);
		}

		public Response GetCommand(string agentSystemName, string commandName)
		{
			var asJson = false;
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

			return
				View[
					"Commands/view-command.cshtml", new CommandModel { AgentSystemName = agentSystemName, CommandName = commandName }];
		}

		public Response GetCommandStatus(Guid publicationId)
		{
			Exception fetchRegistryError = null;
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
				fetchRegistryError = e;
			}

			if (Request.IsAjaxRequest())
			{
				return fetchRegistryError == null
				       	? Response.AsJson(record)
				       	: Response.AsJson(
				       		new
				       			{
				       				name = fetchRegistryError.GetType().Name,
				       				message = fetchRegistryError.Message,
				       				callStack = fetchRegistryError.StackTrace
				       			},
				       		HttpStatusCode.InternalServerError);
			}

			var model = new PublishedCommandModel { PublicationId = publicationId.ToString(), Record = record };
			return View["Commands/view-publication-record.cshtml", model];
		}

		private IInputModel getInputModel(string agentSystemName, string commandName)
		{
			var agent =
				_compositeApp.Agents.Where(a => a.SystemName.Equals(agentSystemName, StringComparison.InvariantCultureIgnoreCase)).
					FirstOrDefault();

			if (agent == null)
			{
				return null;
			}

			var command =
				agent.Commands.Where(c => c.Name.Equals(commandName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

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