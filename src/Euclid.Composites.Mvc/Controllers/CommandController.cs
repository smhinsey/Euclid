using System;
using System.Web.Mvc;
using Euclid.Common.Messaging;
using Euclid.Composites.InputModel;
using Euclid.Composites.Mvc.Maps;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata;

namespace Euclid.Composites.Mvc.Controllers
{
    public class CommandController : Controller
    {
        private readonly MapperRegistry _mapperRegistry;
        private readonly IPublisher _commandPublisher;

        public CommandController(MapperRegistry mapperRegistry, IPublisher commandPublisher)
        {
            _mapperRegistry = mapperRegistry;
            _commandPublisher = commandPublisher;
        }

        public ViewResult List(IAgentMetadata agentMetadata)
        {
            ViewBag.Title = "Commands in agent";

            // jt : CommandMetadata should be a service w/a factory method?

            return View(new CommandMetadataModel { AgentMetadata = agentMetadata });
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

        public ViewResult Inspect(IEuclidMetdata metadata, string extension)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            var mapper = _mapperRegistry.Get<IEuclidMetdata, IInputModel>();
            if (mapper == null)
            {
                throw new MapperNotFoundException(typeof(IEuclidMetdata), typeof(IInputModel));
            }

            var inputModel = mapper.Map(metadata);
            inputModel.SubmittedByUser = HttpContext.User.Identity.Name;
            //inputModel.AgentSystemName = metadata.CommandType.Assembly.GetAgentInfo().SystemName;

            if (inputModel == null)
            {
                throw new MappingFailedException(mapper.GetType(), typeof(IEuclidMetdata), typeof(IInputModel));
            }

            return View(inputModel);
        }
    }
}