using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace ForumComposite
{
	// TODO: all of the below queries need to be combined into a single one
	// TODO: this should only execute once per request
	// TODO: user-specific data should be moved to a principal or something like that
	public class CommonForumInfo
	{
		private readonly CategoryQueries _categoryQueries;

		private readonly ForumQueries _forumQueries;

		private readonly OrganizationQueries _orgQueries;

		private readonly UserQueries _userQueries;

		private readonly ContentQueries _contentQueries;

		private readonly TagQueries _tagQueries;

		public CommonForumInfo()
		{
			_forumQueries = DependencyResolver.Current.GetService<ForumQueries>();
			_orgQueries = DependencyResolver.Current.GetService<OrganizationQueries>();
			_categoryQueries = DependencyResolver.Current.GetService<CategoryQueries>();
			_userQueries = DependencyResolver.Current.GetService<UserQueries>();
			_contentQueries = DependencyResolver.Current.GetService<ContentQueries>();
			_tagQueries = DependencyResolver.Current.GetService<TagQueries>();
		}

		public IList<Category> Categories { get; private set; }

		public Guid AuthenticatedUserIdentifier { get; private set; }

		public string AuthenticatedUserName { get; private set; }

		public Guid ForumIdentifier { get; private set; }

		public string ForumName { get; private set; }

		public Guid OrganizationIdentifier { get; private set; }

		public string OrganizationName { get; private set; }
		
		public string ForumTheme { get; private set; }

		public IList<ForumUser> TopUsers { get; private set; }
		public IList<Tag> Tags { get; private set; }

		public IDictionary<string, ForumContent> CustomContent { get; private set; }

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
			ForumTheme = forum.Theme;

			CustomContent = new Dictionary<string, ForumContent>();

			var content = _contentQueries.GetAllActiveContent(forum.Identifier);

			foreach (var forumContent in content)
			{
				CustomContent.Add(forumContent.ContentLocation, forumContent);
			}

			Categories = _categoryQueries.GetActiveCategories(ForumIdentifier, 0, 100);
			TopUsers = _userQueries.FindTopUsers(ForumIdentifier);
			Tags = _tagQueries.List(ForumIdentifier, 0, 100).Tags;

			if (HttpContext.Current.Request.IsAuthenticated)
			{
				AuthenticatedUserName = HttpContext.Current.User.Identity.Name;

				var cookie = HttpContext.Current.Request.Cookies[string.Format("{0}UserId", ForumName)];
				if (cookie != null)
				{
					AuthenticatedUserIdentifier = Guid.Parse(cookie.Value);
				}
			}
		}
	}
}