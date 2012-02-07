using System.Web.Mvc;

namespace StorefrontAdminComposite
{
	public abstract class AdminViewPage<T> : WebViewPage<T>
	{
		public CommonStorefrontAdminInfo AppInfo { get; private set; }

		protected override void InitializePage()
		{
			var descriptor = new CommonStorefrontAdminInfo();

			descriptor.Initialize(Url.RequestContext.RouteData);

			AppInfo = descriptor;

			base.InitializePage();
		}
	}
}