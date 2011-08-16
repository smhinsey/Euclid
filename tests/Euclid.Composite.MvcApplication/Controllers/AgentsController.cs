using System.Web.Mvc;
using Euclid.Common.Messaging;
using Euclid.Composite.MvcApplication.Models;
using Euclid.Composites;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.ActionFilters;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Composite.MvcApplication.Controllers
{
	public class AgentsController : Controller
	{
		private readonly IPublisher _commandPublisher;
		private readonly ICompositeApp _composite;
		private readonly IInputModelTransfomerRegistry _transformer;

		public AgentsController(ICompositeApp composite, IPublisher commandPublisher, IInputModelTransfomerRegistry transformer)
		{
			_composite = composite;
			_commandPublisher = commandPublisher;
			_transformer = transformer;
		}

		[FormatListOfBasicAgentMetadata]
		public ViewResult Index(string format)
		{
			ViewBag.Title = "Agents in composite";

			return View(new AgentListModel(_composite.Agents));
		}

		[FormatPartMetadataAttribute]
		public ActionResult Inspect(ITypeMetadata typeMetadata, string agentSystemName, string partType, string partName, string format)
		{
			ViewBag.Title = typeMetadata.Name;

			return View(new ReadOnlyPartMetadataModel
			            	{
			            		AgentSystemName = agentSystemName,
			            		TypeMetadata = typeMetadata,
			            		Name = partName,
			            		PartType = partType,
			            		NextActionName = (partType.ToLower() == "commands") ? "InspectCommand" : "Inspect"
			            	});
		}

		[HttpPost]
		public ContentResult Inspect(IInputModel inputModel)
		{
			var command = _transformer.GetCommand(inputModel);

			return Publish(command);
		}

		[FormatInputModel]
		public ActionResult InspectCommand(IInputModel inputModel, ITypeMetadata typeMetadata, string partName, string format)
		{
			ActionResult result = View("InspectCommand", inputModel);

			ViewBag.Title = partName;

			return result;
		}

		[FormatPartCollectionMetadata]
		public ViewResult Parts(IAgentPartMetadataFormatterCollection partCollection, string partType, string format)
		{
			ViewBag.Title = string.Format("{0} in agent {1}", partType, partCollection.AgentSystemName);

			return View(new PartCollectionMetadataModel
			            	{
			            		AgentSystemName = partCollection.AgentSystemName,
			            		Parts = partCollection,
			            		PartTypeName = partType,
			            		NextActionName = (partType.ToLower() == "commands") ? "InspectCommand" : "Inspect"
			            	});
		}

		[FormatAgentMetadata]
		public ViewResult ViewAgent(IAgentMetadataFormatter agentMetadataFormatter, string format)
		{
			ViewBag.Title = agentMetadataFormatter.SystemName;

			return View(new AgentModel
			            	{
			            		DescriptiveName = agentMetadataFormatter.DescriptiveName,
			            		SystemName = agentMetadataFormatter.SystemName,
			            		Commands = new AgentPartModel
			            		           	{
			            		           		AgentSystemName = agentMetadataFormatter.SystemName,
			            		           		PartMetadataFormatter = agentMetadataFormatter.Commands,
			            		           		PartType = "Commands",
			            		           		NextAction = "InspectCommand"
			            		           	},
			            		Queries = new AgentPartModel
			            		          	{
			            		          		AgentSystemName = agentMetadataFormatter.SystemName,
			            		          		PartMetadataFormatter = agentMetadataFormatter.Queries,
			            		          		PartType = "Queries",
			            		          		NextAction = "Inspect"
			            		          	},
			            		ReadModels = new AgentPartModel
			            		             	{
			            		             		AgentSystemName = agentMetadataFormatter.SystemName,
			            		             		PartMetadataFormatter = agentMetadataFormatter.ReadModels,
			            		             		PartType = "ReadModels",
			            		             		NextAction = "Inspect"
			            		             	}
			            	});
		}

		private ContentResult Publish(ICommand command)
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
	}
}