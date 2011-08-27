using System.Web.Mvc;
using CompositeInspector.Models;
using Euclid.Common.Messaging;
using Euclid.Composites;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.ActionFilters;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.Models;
using MvcContrib.Filters;

namespace CompositeInspector.Controllers
{
	[Layout("_Layout")]
	public class AgentsController : Controller
	{
		private readonly IPublisher _commandPublisher;

		private readonly ICompositeApp _composite;

		private readonly IInputModelTransformerRegistry _transformer;

		public AgentsController(
			ICompositeApp composite, IPublisher commandPublisher, IInputModelTransformerRegistry transformer)
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

		[HttpPost]
		public ActionResult Publish(IInputModel inputModel)
		{
			var command = _transformer.GetCommand(inputModel);

			if (command == null)
			{
				return new ContentResult { Content = "No command to publish" };
			}

			var commandId = _commandPublisher.PublishMessage(command);

			return RedirectToAction("Details", "CommandRegistry", new { publicationId = commandId });
		}

		[FormatAgentMetadata]
		public ViewResult ViewAgent(IAgentMetadata agentMetadata, string format)
		{
			return
				View(
					new AgentModel
						{
							AgentSytemName = agentMetadata.SystemName,
							DescriptiveName = agentMetadata.DescriptiveName,
							Description = agentMetadata.Description,
							Commands =
								new AgentPartModel
									{
										AgentSystemName = agentMetadata.SystemName,
										NextAction = "ViewInputModelForCommand",
										Part = agentMetadata.Commands
									},
							Queries =
								new AgentPartModel
									{ AgentSystemName = agentMetadata.SystemName, NextAction = "ViewPart", Part = agentMetadata.Queries },
							ReadModels =
								new AgentPartModel
									{ AgentSystemName = agentMetadata.SystemName, NextAction = "ViewPart", Part = agentMetadata.ReadModels },
						});
		}

		[FormatInputModel]
		public ActionResult ViewInputModelForCommand(IInputModel inputModel, IPartMetadata typeMetadata, string format)
		{
			var partCollection = typeMetadata.GetContainingPartCollection();

			if (inputModel == null)
			{
				return RedirectToAction(
					"ViewPart", new { partCollection.AgentSystemName, PartName = typeMetadata.Name, Format = format });
			}

			ViewBag.Title = typeMetadata.Type.Name;

			ViewBag.Navigation = new InspectorNavigationModel
				{
					AgentSytemName = partCollection.AgentSystemName,
					PartDescriptiveName = partCollection.DescriptiveName,
					PartType = typeMetadata.Type.Name
				};

			ActionResult result = View("ViewInputModelForCommand", inputModel);

			return result;
		}

		[FormatPartMetadataAttribute]
		public ActionResult ViewPart(ITypeMetadata typeMetadata, IPartCollection containingCollection, string format)
		{
			ViewBag.Title = typeMetadata.Name;

			return
				View(
					new PartModel
						{
							TypeMetadata = typeMetadata,
							NextActionName = (containingCollection.DescriptiveName == "Commands") ? "ViewInputModelForCommand" : "ViewPart",
							AgentSytemName = containingCollection.AgentSystemName,
							PartDescriptiveName = containingCollection.DescriptiveName,
							PartType = typeMetadata.Type.Name
						});
		}

		[FormatPartCollectionMetadata]
		public ViewResult ViewPartCollection(IPartCollection partCollection, string format)
		{
			ViewBag.Title = string.Format("Agent {0}", partCollection.DescriptiveName);

			return
				View(
					new PartCollectionModel
						{
							Parts = partCollection,
							NextActionName = (partCollection.DescriptiveName == "Commands") ? "ViewInputModelForCommand" : "ViewPart",
							AgentSytemName = partCollection.AgentSystemName,
							PartDescriptiveName = partCollection.DescriptiveName
						});
		}
	}
}