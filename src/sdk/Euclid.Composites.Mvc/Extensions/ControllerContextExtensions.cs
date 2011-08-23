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

		public static string GetPartDescriptiveName(this ControllerContext controllerContext)
		{
			return controllerContext.GetRouteValue<string>("DescriptiveName");
		}

		public static string GetPartName(this ControllerContext controllerContext)
		{
			return controllerContext.GetRouteValue<string>("PartName");
		}

		public static T GetRouteValue<T>(this ControllerContext controllerContext, string key)
		{
			var value = controllerContext.RouteData.Values[key];

			return (T)value;
		}
	}
}