using System.Web.Mvc;

namespace ForumComposite
{
	public abstract class ForumViewPage<T> : WebViewPage<T>
	{
		public SharedForumInfo ForumInfo { get; private set; }

		protected override void InitializePage()
		{
			var descriptor = new SharedForumInfo();

			descriptor.Initialize(Url.RequestContext.RouteData);

			ForumInfo = descriptor;

			base.InitializePage();
		}
	}
}