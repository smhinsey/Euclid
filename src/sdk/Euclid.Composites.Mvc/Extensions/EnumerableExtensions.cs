using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Euclid.Composites.Mvc.Extensions
{
	public static class EnumerableExtensions
	{
		public static void SetupPaging<T>(this IEnumerable<T> entries, Controller controller, int pageSize, int offset)
		{
			var count = entries.Count();
			var controllerName = controller.RouteData.Values["controller"].ToString();
			var actionName = controller.RouteData.Values["action"].ToString();

			controller.ViewBag.PageSize = pageSize;

			controller.ViewBag.Offset = offset;

			controller.ViewBag.TotalPages = (count / pageSize) + 1;

			controller.ViewBag.CurrentPage = offset + 1;

			controller.ViewBag.HasPreviousPage = (offset > 0);

			controller.ViewBag.HasNextPage = count > (offset + 1) * pageSize;

			controller.ViewBag.NextPageUrl = (!controller.ViewBag.HasNextPage)
			                                 	? "#"
			                                 	: controller.Url.Action(
			                                 		actionName, controllerName, new { pageSize, offset = offset + 1 });

			controller.ViewBag.PreviousPageUrl = (!controller.ViewBag.HasPreviousPage)
			                                     	? "#"
			                                     	: controller.Url.Action(
			                                     		actionName, controllerName, new { pageSize, offset = offset - 1 });
		}
	}
}