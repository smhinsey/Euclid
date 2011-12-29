using System;
using System.Web.Mvc;
using ForumAgent.Queries;

namespace ForumComposite
{
	public abstract class ForumViewPage<T> : WebViewPage<T>
	{
		private readonly ForumQueries _forumQueries;

		private readonly OrganizationQueries _orgQueries;

		protected ForumViewPage()
		{
			// TODO we need something better than this
			_forumQueries = DependencyResolver.Current.GetService<ForumQueries>();
			_orgQueries = DependencyResolver.Current.GetService<OrganizationQueries>();
		}

		public Guid ForumIdentifier { get; set; }

		public string ForumName { get; set; }

		public Guid OrganizationIdentifier { get; set; }

		public string OrganizationName { get; set; }

		protected override void InitializePage()
		{
			setGlobalProperties();

			base.InitializePage();
		}

		private void setGlobalProperties()
		{
			var orgSlug = Url.RequestContext.RouteData.Values["org"].ToString();
			var forumSlug = Url.RequestContext.RouteData.Values["forum"].ToString();

			OrganizationIdentifier = _orgQueries.GetIdentifierBySlug(orgSlug);

			var forum = _forumQueries.GetForumBySlug(OrganizationIdentifier, forumSlug);

			ForumName = forum.Name;
			ForumIdentifier = forum.Identifier;
		}
	}
}