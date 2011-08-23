using System.Web.Mvc;
using Euclid.Common.Messaging;
using Euclid.Composite.MvcApplication.Models;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.ActionFilters;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Composite.MvcApplication.Controllers
{
	public class CommandController : Controller
	{
		private readonly IPublisher _commandPublisher;
		private readonly IInputModelTransfomerRegistry _transformer;

		public CommandController(IPublisher commandPublisher, IInputModelTransfomerRegistry transformer)
		{
			_commandPublisher = commandPublisher;
			_transformer = transformer;
		}

		[HttpPost]
		public ContentResult Inspect(IInputModel inputModel)
		{
			var command = _transformer.GetCommand(inputModel);
			return Publish(command);
		}

		[FormatInputModel]
		public ActionResult Inspect(IInputModel inputModel, string commandName, string format)
		{
			ActionResult result = View(inputModel);

			return result;
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

		public ViewResult List(IAgentMetadata agentMetadata)
		{
			ViewBag.Title = "Commands in agent";
			return View(new CommandMetadataModel {AgentSystemName = agentMetadata.SystemName, Commands = agentMetadata.Commands});
		}

	}
}