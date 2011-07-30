using System.Web.Mvc;

namespace Euclid.Composites.Extensions
{
    public static class ControllerContextExtensions
    {
        public static T GetRouteValue<T>(this ControllerContext controllerContext, string valueName)
        {
            var value = controllerContext.RouteData.Values[valueName];

            if (value == null)
            {
                throw new MissingRouteDataException(valueName);
            }

            return (T)value;
        }

        // important route values
        public static string GetAgentSystemName(this ControllerContext controllerContext)
        {
            return controllerContext.GetRouteValue<string>("agentSystemName");
        }

        public static string GetCommandName(this ControllerContext controllerContext)
        {
            return controllerContext.GetRouteValue<string>("commandName");
        }

        public static string GetAction(this ControllerContext controllerContext)
        {
            return controllerContext.GetRouteValue<string>("action");
        }

    }
}