using System.Web.Mvc;

namespace ForumComposite
{
	public abstract class ForumViewPage<T> : WebViewPage<T>
	{
		public CommonForumInfo ForumInfo { get; private set; }

		protected override void InitializePage()
		{
			var descriptor = new CommonForumInfo();

			descriptor.Initialize(Url.RequestContext.RouteData);

			ForumInfo = descriptor;

			base.InitializePage();
		}
	}
}