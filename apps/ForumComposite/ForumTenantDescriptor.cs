using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace ForumComposite
{
	// TODO: this should replace the duplicated properties from ForumViewPage and ForumController
	// TODO: all of the below queries need to be combined into a single one
	public class ForumTenantDescriptor
	{
		private readonly CategoryQueries _categoryQueries;

		private readonly ForumQueries _forumQueries;

		private readonly OrganizationQueries _orgQueries;

		private readonly UserQueries _userQueries;

		public ForumTenantDescriptor()
		{
			_forumQueries = DependencyResolver.Current.GetService<ForumQueries>();
			_orgQueries = DependencyResolver.Current.GetService<OrganizationQueries>();
			_categoryQueries = DependencyResolver.Current.GetService<CategoryQueries>();
			_userQueries = DependencyResolver.Current.GetService<UserQueries>();
		}

		public IList<Category> Categories { get; private set; }

		public Guid CurrentUserIdentifier { get; private set; }

		public string CurrentUserName { get; private set; }

		public Guid ForumIdentifier { get; private set; }

		public string ForumName { get; private set; }

		public Guid OrganizationIdentifier { get; private set; }

		public string OrganizationName { get; private set; }

		public IList<ForumUser> TopUsers { get; private set; }

		public void Initialize(RouteData routeData)
		{
			var orgSlug = routeData.Values["org"].ToString();
			var forumSlug = routeData.Values["forum"].ToString();

			var org = _orgQueries.FindBySlug(orgSlug);
			var forum = _forumQueries.FindBySlug(org.Identifier, forumSlug);

			OrganizationIdentifier = org.Identifier;
			OrganizationName = org.Name;

			ForumName = forum.Name;
			ForumIdentifier = forum.Identifier;

			Categories = _categoryQueries.GetActiveCategories(ForumIdentifier, 0, 100);
			TopUsers = _userQueries.FindTopUsers(ForumIdentifier);

			if (HttpContext.Current.Request.IsAuthenticated)
			{
				CurrentUserName = HttpContext.Current.User.Identity.Name;

				var cookie = HttpContext.Current.Request.Cookies[string.Format("{0}UserId", ForumName)];
				if (cookie != null)
				{
					CurrentUserIdentifier = Guid.Parse(cookie.Value);
				}
			}
		}
	}
}