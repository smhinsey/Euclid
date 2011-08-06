using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Euclid.Framework.Metadata;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Extensions
{
    public static class ControllerContextExtensions
    {
        public static T GetRouteValue<T>(this ControllerContext controllerContext, string key)
        {
            var value = controllerContext.RouteData.Values[key];

            if (value == null)
            {
                throw new RequiredRouteDataMissingException(key);
            }

            return (T)value;
        }

        // important route values
        public static string GetAgentSystemName(this ControllerContext controllerContext)
        {
            return controllerContext.GetRouteValue<string>("AgentSystemName");
        }

        public static string GetCommandName(this ControllerContext controllerContext)
        {
            return controllerContext.GetRouteValue<string>("CommandName");
        }

        public static string GetAction(this ControllerContext controllerContext)
        {
            return controllerContext.GetRouteValue<string>("action");
        }
    }
}