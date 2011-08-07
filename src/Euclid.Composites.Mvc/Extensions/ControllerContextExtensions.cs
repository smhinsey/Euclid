using System.Web.Mvc;

namespace Euclid.Composites.Mvc.Extensions
{
	public static class ControllerContextExtensions
	{
		// important route values
		public static string GetAction(this ControllerContext controllerContext)
		{
			return controllerContext.GetRouteValue<string>("action");
		}

		public static string GetAgentSystemName(this ControllerContext controllerContext)
		{
			return controllerContext.GetRouteValue<string>("AgentSystemName");
		}

		public static string GetCommandName(this ControllerContext controllerContext)
		{
			return controllerContext.GetRouteValue<string>("CommandName");
		}

		public static T GetRouteValue<T>(this ControllerContext controllerContext, string key)
		{
			var value = controllerContext.RouteData.Values[key];

			if (value == null)
			{
				throw new RequiredRouteDataMissingException(key);
			}

			return (T) value;
		}
	}
}