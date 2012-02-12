using System.Web.Mvc;

namespace AdminComposite
{
	public abstract class AdminViewPage<T> : WebViewPage<T>
	{
		public CommonAdminInfo AdminInfo { get; private set; }

		protected override void InitializePage()
		{
			var descriptor = new CommonAdminInfo();

			descriptor.Initialize(Url.RequestContext.RouteData);

			AdminInfo = descriptor;

			base.InitializePage();
		}
	}
}