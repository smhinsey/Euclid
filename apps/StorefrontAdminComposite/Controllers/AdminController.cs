using System.Web.Mvc;
using System.Web.Routing;
using Euclid.Common.Messaging;

namespace StorefrontAdminComposite.Controllers
{
	public abstract class AdminController : Controller
	{
		protected AdminController()
		{
			Publisher = DependencyResolver.Current.GetService<IPublisher>();
		}

		public CommonStorefrontAdminInfo AppInfo { get; private set; }

		public IPublisher Publisher { get; private set; }

		protected override void Execute(RequestContext requestContext)
		{
			var descriptor = new CommonStorefrontAdminInfo();

			descriptor.Initialize(requestContext.RouteData);

			AppInfo = descriptor;

			base.Execute(requestContext);
		}
	}
}