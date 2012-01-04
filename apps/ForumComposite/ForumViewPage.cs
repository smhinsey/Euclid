using System.Web.Mvc;

namespace ForumComposite
{
	public abstract class ForumViewPage<T> : WebViewPage<T>
	{
		public ForumTenantDescriptor CurrentForum { get; private set; }

		protected override void InitializePage()
		{
			var descriptor = new ForumTenantDescriptor();

			descriptor.Initialize(Url.RequestContext.RouteData);

			CurrentForum = descriptor;

			base.InitializePage();
		}
	}
}