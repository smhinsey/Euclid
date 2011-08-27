using System.Web.Mvc;
using Castle.Windsor;
using Euclid.Composites.Mvc.Extensions;

namespace Euclid.Composites.Mvc.ActionFilters
{
    public class EuclidActionInvoker : ControllerActionInvoker
    {
        private readonly IWindsorContainer _container;

        public EuclidActionInvoker(IWindsorContainer container)
        {
            _container = container;
        }

        protected override ActionExecutedContext InvokeActionMethodWithFilters(ControllerContext controllerContext, System.Collections.Generic.IList<IActionFilter> filters, ActionDescriptor actionDescriptor, System.Collections.Generic.IDictionary<string, object> parameters)
        {
            foreach(var filter in filters)
            {
                _container.InjectProperties(filter);
            }

            return base.InvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters);
        }
    }
}