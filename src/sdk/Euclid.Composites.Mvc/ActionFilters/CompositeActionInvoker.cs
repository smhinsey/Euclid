using System.Collections.Generic;
using System.Web.Mvc;
using Castle.Windsor;
using Euclid.Composites.Mvc.Extensions;

namespace Euclid.Composites.Mvc.ActionFilters
{
	public class CompositeActionInvoker : ControllerActionInvoker
	{
		private readonly IWindsorContainer _container;

		public CompositeActionInvoker(IWindsorContainer container)
		{
			_container = container;
		}

		protected override ActionExecutedContext InvokeActionMethodWithFilters(
			ControllerContext controllerContext,
			IList<IActionFilter> filters,
			ActionDescriptor actionDescriptor,
			IDictionary<string, object> parameters)
		{
			foreach (var filter in filters)
			{
				_container.InjectProperties(filter);
			}

			return base.InvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters);
		}
	}
}