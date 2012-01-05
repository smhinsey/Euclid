using System;
using System.IO;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Euclid.Common.Messaging;
using Euclid.Composites;
using Euclid.Composites.Conversion;
using Euclid.Framework.AgentMetadata.Formatters;
using Euclid.Framework.Models;
using JsonCompositeInspector.Models;
using Nancy;
using Nancy.ModelBinding;

namespace JsonCompositeInspector.Module
{
	public class CommandModule : NancyModule
	{
		private readonly IWindsorContainer _container;
		private readonly ICompositeApp _compositeApp;
		private readonly IPublisher _publisher;

		public CommandModule(IWindsorContainer container)
			: base("composite/commands")
		{
			_container = container;
			_compositeApp = _container.Resolve<ICompositeApp>();
			_publisher = _container.Resolve<IPublisher>();
			Get[""] = _ => "Command API";

			Get["/{agentSystemName}/{commandName}"] = p =>
			                                          	{
			                                          		var agentSystemName = (string) p.agentSystemName;
			                                          		var commandName = (string) p.commandName;

			                                          		var asJson = false;
			                                          		if (commandName.EndsWith(".json"))
			                                          		{
			                                          			asJson = true;
			                                          			commandName = commandName.Substring(0, commandName.Length - 5);
			                                          		}

			                                          		var inputModel = getInputModel(agentSystemName, commandName);

			                                          		if (inputModel == null)
			                                          		{
			                                          			return
			                                          				string.Format(
			                                          					"The {0} composite application does not support the command '{1}'",
			                                          					_compositeApp.Name, commandName);
			                                          		}

			                                          		if (asJson)
			                                          		{
			                                          			var f = new InputModelFormatter(inputModel);
			                                          			var s = new MemoryStream(Encoding.UTF8.GetBytes(f.GetAsJson()));
			                                          			return Response.FromStream(s, "application/json");
			                                          		}

			                                          		return View["Commands/view-command.cshtml", new CommandModel
			                                          		                                           	{
			                                          		                                           		AgentSystemName =
			                                          		                                           			agentSystemName,
			                                          		                                           		CommandName = commandName
			                                          		                                           	}];
			                                          	};

			Post["/publish"] = p =>
			                   	{
			                   		var inputModel = this.Bind<IInputModel>();
			                   		if (inputModel == null)
			                   		{
			                   			throw new InvalidOperationException("Unable to retrieve input model from form");
			                   		}

			                   		var command = _compositeApp.GetCommandForInputModel(inputModel);
			                   		return _publisher.PublishMessage(command).ToString();
			                   		//var type = inputModel.GetType();
			                   		//var sb = new StringBuilder(type.Name);
			                   		//foreach (var property in type.GetProperties())
			                   		//{
			                   		//    var v = property.GetValue(inputModel, null);
			                   		//    sb.AppendFormat("<br/>&nbsp;&nbsp;.{0} = {1}", property.Name, v);
			                   		//}

			                   		//return sb.ToString();
			                   		//};
			                   	};
		}

		private IInputModel getInputModel(string agentSystemName, string commandName)
		{
			var agent = _compositeApp
							.Agents
							.Where(a => a.SystemName.Equals(agentSystemName, StringComparison.InvariantCultureIgnoreCase))
							.FirstOrDefault();

			if (agent == null) return null;

			var command = agent
							.Commands
							.Where(c =>c.Name.Equals(commandName,StringComparison.InvariantCultureIgnoreCase))
							.FirstOrDefault();

			if (command == null) return null;

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