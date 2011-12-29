using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace ForumComposite
{
	// TODO: all of the below queries need to reside elsewhere
	// TODO: all of the below queries need to be combined into a single one
	public abstract class ForumViewPage<T> : WebViewPage<T>
	{
		private readonly ForumQueries _forumQueries;

		private readonly OrganizationQueries _orgQueries;
		private readonly CategoryQueries _categoryQueries;
		private readonly UserQueries _userQueries;

		protected ForumViewPage()
		{
			_forumQueries = DependencyResolver.Current.GetService<ForumQueries>();
			_orgQueries = DependencyResolver.Current.GetService<OrganizationQueries>();
			_categoryQueries = DependencyResolver.Current.GetService<CategoryQueries>();
			_userQueries = DependencyResolver.Current.GetService<UserQueries>();
		}

		public Guid ForumIdentifier { get; set; }

		public string ForumName { get; set; }

		public Guid OrganizationIdentifier { get; set; }

		public string OrganizationName { get; set; }

		public IList<Category> Categories { get; set; }
		public IList<ForumUser> TopUsers { get; set; }

		protected override void InitializePage()
		{
			setGlobalProperties();

			base.InitializePage();
		}

		private void setGlobalProperties()
		{
			var orgSlug = Url.RequestContext.RouteData.Values["org"].ToString();
			var forumSlug = Url.RequestContext.RouteData.Values["forum"].ToString();

			var org = _orgQueries.FindBySlug(orgSlug);
			var forum = _forumQueries.FindBySlug(org.Identifier, forumSlug);

			OrganizationIdentifier = org.Identifier;
			OrganizationName = org.Name;

			ForumName = forum.Name;
			ForumIdentifier = forum.Identifier;

			Categories = _categoryQueries.GetActiveCategories(ForumIdentifier, 0, 100);
			TopUsers = _userQueries.FindTopUsers(ForumIdentifier);
		}
	}
}