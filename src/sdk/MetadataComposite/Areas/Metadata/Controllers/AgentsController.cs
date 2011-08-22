using System.Web.Mvc;
using Euclid.Common.Messaging;
using Euclid.Composites;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.ActionFilters;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using MetadataComposite.Areas.Metadata.Models;

namespace MetadataComposite.Areas.Metadata.Controllers
{
	public class AgentsController : Controller
	{
		private readonly IPublisher _commandPublisher;

		private readonly ICompositeApp _composite;

		private readonly IInputModelTransfomerRegistry _transformer;

		public AgentsController(
			ICompositeApp composite, IPublisher commandPublisher, IInputModelTransfomerRegistry transformer)
		{
			this._composite = composite;
			this._commandPublisher = commandPublisher;
			this._transformer = transformer;
		}

		[FormatListOfBasicAgentMetadata]
		public ViewResult Index(string format)
		{
			this.ViewBag.Title = "Agents in composite";

			return this.View(new AgentListModel(this._composite.Agents));
		}

		[HttpPost]
		public ContentResult Inspect(IInputModel inputModel)
		{
			var command = this._transformer.GetCommand(inputModel);

			// smh: it's pretty surprising that Inspect calls Publish. i think this should be really explicit.
			return this.Publish(command);
		}

		[FormatAgentMetadata]
		public ViewResult ViewAgent(IAgentMetadata agentMetadata, string format)
		{
			this.ViewBag.Title = agentMetadata.SystemName;

			return
				this.View(
					new AgentModel
						{
							DescriptiveName = agentMetadata.DescriptiveName, 
							SystemName = agentMetadata.SystemName, 
							Commands =
								new AgentPartModel
									{
										AgentSystemName = agentMetadata.SystemName, 
										NextAction = "ViewInputModelForCommand", 
										Part = agentMetadata.Commands
									}, 
							Queries =
								new AgentPartModel
									{
            AgentSystemName = agentMetadata.SystemName, NextAction = "ViewPart", Part = agentMetadata.Queries 
         }, 
							ReadModels =
								new AgentPartModel
									{
            AgentSystemName = agentMetadata.SystemName, NextAction = "ViewPart", Part = agentMetadata.ReadModels 
         }, 
							AgentSytemName = agentMetadata.SystemName
						});
		}

		[FormatInputModel]
		public ActionResult ViewInputModelForCommand(IInputModel inputModel, ITypeMetadata typeMetadata, string format)
		{
			var partCollection = typeMetadata.GetContainingPartCollection();

			if (inputModel == null)
			{
				return this.RedirectToAction(
					"ViewPart", new { partCollection.AgentSystemName, PartName = typeMetadata.Name, Format = format });
			}

			this.ViewBag.Title = typeMetadata.Type.Name;

			this.ViewBag.Navigation = new FooterLinkModel
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
			this.ViewBag.Title = typeMetadata.Name;

			return
				this.View(
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
			this.ViewBag.Title = string.Format("Agent {0}", partCollection.DescriptiveName);

			return
				this.View(
					new PartCollectionModel
						{
							Parts = partCollection, 
							NextActionName = (partCollection.DescriptiveName == "Commands") ? "ViewInputModelForCommand" : "ViewPart", 
							AgentSytemName = partCollection.AgentSystemName, 
							PartDescriptiveName = partCollection.DescriptiveName
						});
		}

		private ContentResult Publish(ICommand command)
		{
			if (command == null)
			{
				return new ContentResult { Content = "No command to publish" };
			}

			var commandId = this._commandPublisher.PublishMessage(command);

			return new ContentResult { Content = commandId.ToString() };
		}
	}
}