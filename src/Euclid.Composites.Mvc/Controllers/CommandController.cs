using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Euclid.Common.Messaging;
using Euclid.Composites.Extensions;
using Euclid.Composites.InputModel;
using Euclid.Composites.Mvc.MappingPipeline;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.Metadata;

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

        public ViewResult List(IEnumerable<ICommandMetadata> commands, IAgentInfo agentInfo)
        {
            ViewBag.Title = "Commands in agent";

            return View(commands.Select(x=>new CommandMetadataModel { AgentInfo = agentInfo, CommandMetadata = x}));
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

        public ViewResult Inspect(ICommandMetadata metadata, string extension)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            var mapper = _mapperRegistry.Get<ICommandMetadata, IInputModel>();
            if (mapper == null)
            {
                throw new MapperNotFoundException(typeof(ICommandMetadata), typeof(IInputModel));
            }

            var inputModel = mapper.Map(metadata);
            inputModel.SubmittedByUser = HttpContext.User.Identity.Name;
            inputModel.AgentSystemName = metadata.CommandType.Assembly.GetAgentInfo().SystemName;

            if (inputModel == null)
            {
                throw new MappingFailedException(mapper.GetType(), typeof(ICommandMetadata), typeof(IInputModel));
            }

            return View(inputModel);
        }
    }
}