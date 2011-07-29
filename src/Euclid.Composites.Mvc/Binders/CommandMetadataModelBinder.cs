
D:\Projects\Euclid\platform>@git.exe %*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Euclid.Agent;
using Euclid.Composites.Agent;
using Euclid.Composites.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc.Binders
{
    public class CommandMetadataModelBinder : IEuclidModelBinder
    {
        private readonly IAgentResolutionStrategy[] _resolvers;

        public CommandMetadataModelBinder(IAgentResolutionStrategy[] resolvers)
        {
            _resolvers = resolvers;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var agent = GetAgent(controllerContext);

            var command = (string)controllerContext.RouteData.Values["command"];

            var action = ((string)controllerContext.RouteData.Values["action"]).ToLower();

            if (action == "list")
            {
                return agent.GetCommands();
            }
            else if (action == "inspect")
            {
                return agent.GetCommand(command);
            }

            return null;
        }

        private Assembly GetAgent(ControllerContext controllerContext)
        {
            var scheme = (string) controllerContext.RouteData.Values["scheme"];

            var systemName = (string) controllerContext.RouteData.Values["systemName"];

            if (string.IsNullOrEmpty(scheme) || string.IsNullOrEmpty(systemName)) return null;

            var agent =
                _resolvers.Select(rslvr => rslvr.GetAgent(scheme, systemName)).FirstOrDefault(assembly => assembly != null);

            if (agent == null)
            {
                throw new AgentNotFoundException(scheme, systemName);
            }

            return agent;
        }

        public bool IsMatch(Type modelType)
        {
            return modelType == typeof (ICommandMetadata) ||
                    modelType == typeof(IEnumerable<ICommandMetadata>);
        }
    }
}
D:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

D:\Projects\Euclid\platform>@rem Restore the original console codepage.

D:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
