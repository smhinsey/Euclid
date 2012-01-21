using System.Web.Mvc;
using System.Web.Routing;
using Euclid.Common.Messaging;

namespace AdminComposite.Controllers
{
	public abstract class AdminController : Controller
	{
		protected AdminController()
		{
			Publisher = DependencyResolver.Current.GetService<IPublisher>();
		}

		public CommonAdminInfo AdminInfo { get; private set; }

		public IPublisher Publisher { get; private set; }

		protected override void Execute(RequestContext requestContext)
		{
			var descriptor = new CommonAdminInfo();

			descriptor.Initialize(requestContext.RouteData);

			AdminInfo = descriptor;

			base.Execute(requestContext);
		}
	}
}