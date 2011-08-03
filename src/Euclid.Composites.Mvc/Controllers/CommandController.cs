using System;
using System.Web.Mvc;
using Euclid.Common.Messaging;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Controllers
{
    public class CommandController : Controller
    {
        private readonly IPublisher _commandPublisher;

        public CommandController(IPublisher commandPublisher)
        {
            _commandPublisher = commandPublisher;
        }

        public ViewResult List(IAgentMetadata agentMetadata)
        {
            ViewBag.Title = "Commands in agent";
            return
                View(new CommandMetadataModel
                         {AgentSystemName = agentMetadata.SystemName, Commands = agentMetadata.Commands});
        }

        public ContentResult Publish(ICommand command)
        {
            if (command == null)
            {
                return new ContentResult
                           {
                               Content = "No command to publish"
                           };
            }

            var commandId = _commandPublisher.PublishMessage(command);

            return new ContentResult
                       {
                           Content = commandId.ToString()
                       };
        }

        [HttpPost]
        public ContentResult Inspect(ICommand command)
        { 
            return Publish(command);
        }

        public ViewResult Inspect(IInputModel inputModel, string commandName, string extension)
        {
            return View(inputModel);
        }
    }
}