using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
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

		private readonly ContentQueries _contentQueries;

		private readonly ForumQueries _forumQueries;

		private readonly OrganizationQueries _orgQueries;

		private readonly TagQueries _tagQueries;

		private readonly UserQueries _userQueries;

		public CommonForumInfo()
		{
			_forumQueries = DependencyResolver.Current.GetService<ForumQueries>();
			_orgQueries = DependencyResolver.Current.GetService<OrganizationQueries>();
			_categoryQueries = DependencyResolver.Current.GetService<CategoryQueries>();
			_userQueries = DependencyResolver.Current.GetService<UserQueries>();
			_contentQueries = DependencyResolver.Current.GetService<ContentQueries>();
			_tagQueries = DependencyResolver.Current.GetService<TagQueries>();
		}

		public Guid AuthenticatedUserIdentifier { get; private set; }

		public string AuthenticatedUserName { get; private set; }

		public IList<Category> Categories { get; private set; }

		public IDictionary<string, ForumContent> CustomContent { get; private set; }

		public Guid ForumIdentifier { get; private set; }

		public string ForumName { get; private set; }

		public string ForumTheme { get; private set; }

		public Guid OrganizationIdentifier { get; private set; }

		public string OrganizationName { get; private set; }

		public IList<Tag> Tags { get; private set; }

		public IList<ForumUser> TopUsers { get; private set; }

		public void Initialize(RouteData routeData)
		{
			var orgSlug = routeData.Values["org"].ToString();
			var forumSlug = routeData.Values["forum"].ToString();

			var cacheKey = string.Format("{0}^{1}", orgSlug, forumSlug);

			if (HttpContext.Current.Cache[cacheKey] != null)
			{
				var instance = (CommonForumInfo)HttpContext.Current.Cache[cacheKey];

				OrganizationIdentifier = instance.OrganizationIdentifier;
				OrganizationName = instance.OrganizationName;

				ForumName = instance.ForumName;
				ForumIdentifier = instance.ForumIdentifier;
				ForumTheme = instance.ForumTheme;

				CustomContent = instance.CustomContent;

				Categories = instance.Categories;
				TopUsers = instance.TopUsers;
				Tags = instance.Tags;
			}
			else
			{
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
				Tags = _tagQueries.FindActiveTags(ForumIdentifier, 0, 100).Tags;

				var absoluteExpiration = DateTime.Now.AddMinutes(5);

				HttpContext.Current.Cache.Add(cacheKey, this, null, absoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
			}

			if (HttpContext.Current.Request.IsAuthenticated)
			{
				AuthenticatedUserName = HttpContext.Current.User.Identity.Name;

				var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

				if (cookie != null)
				{
					var ticket = FormsAuthentication.Decrypt(cookie.Value);

					var parts = ticket.UserData.Split(new[] { "^" }, StringSplitOptions.None);

					if (parts.Length == 3)
					{
						AuthenticatedUserIdentifier = Guid.Parse(parts[2]);
					}
					else
					{
						throw new Exception("Invalid or corrupt authentication cookie.");
					}
				}
			}
		}
	}
}