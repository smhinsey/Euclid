using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using Euclid.Common.Extensions;
using ForumAgent;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace AdminComposite
{
	// TODO: all of the below queries need to be combined into a single one
	// TODO: this should only execute once per request
	// TODO: user-specific data should be moved to a principal or something like that
	public class CommonAdminInfo
	{
		private readonly CategoryQueries _categoryQueries;

		private readonly ContentQueries _contentQueries;

		private readonly ForumQueries _forumQueries;

		private readonly OrganizationQueries _orgQueries;

		private readonly OrganizationUserQueries _orgUserQueries;

		private readonly TagQueries _tagQueries;

		public CommonAdminInfo()
		{
			_forumQueries = DependencyResolver.Current.GetService<ForumQueries>();
			_orgQueries = DependencyResolver.Current.GetService<OrganizationQueries>();
			_categoryQueries = DependencyResolver.Current.GetService<CategoryQueries>();
			_orgUserQueries = DependencyResolver.Current.GetService<OrganizationUserQueries>();
			_contentQueries = DependencyResolver.Current.GetService<ContentQueries>();
			_tagQueries = DependencyResolver.Current.GetService<TagQueries>();
		}

		public string AuthenticatedUserFirstName { get; set; }

		public string AuthenticatedUserGravatar { get; set; }

		public Guid AuthenticatedUserIdentifier { get; set; }

		public string AuthenticatedUserLastName { get; set; }

		public string AuthenticatedUserName { get; set; }

		public string CurrentForumId { get; set; }

		public IList<Forum> Forums { get; set; }

		public Guid OrganizationId { get; set; }

		public string OrganizationSlug { get; set; }
		
		public string OrganizationName { get; set; }

		public void Initialize(RouteData routeData)
		{
			if (HttpContext.Current.Request.IsAuthenticated)
			{
				var currentUser = _orgUserQueries.FindByUsername(HttpContext.Current.User.Identity.Name);

				CurrentForumId = HttpContext.Current.Request.Params["forumId"];

				var cacheKey = string.Format("CommonAdminInfo-{0}", CurrentForumId);

				if (HttpContext.Current.Cache[cacheKey] != null)
				{
					var cachedInfo = (CommonAdminInfo)HttpContext.Current.Cache[cacheKey];

					Forums = cachedInfo.Forums;
					CurrentForumId = cachedInfo.CurrentForumId;
					OrganizationId = cachedInfo.OrganizationId;
					OrganizationSlug = cachedInfo.OrganizationSlug;
					OrganizationName = cachedInfo.OrganizationName;
				}
				else
				{
					var organization = _orgQueries.FindById(currentUser.OrganizationIdentifier);

					if (organization == null)
					{
						throw new OrganizationNotFoundException(currentUser.OrganizationIdentifier);
					}

					Forums = _forumQueries.FindByOrganization(organization.Identifier);
					OrganizationId = organization.Identifier;
					OrganizationSlug = organization.Slug;
					OrganizationName = organization.Name;

					var absoluteExpiration = DateTime.Now.AddMinutes(5);

					HttpContext.Current.Cache.Add(
						cacheKey, this, null, absoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
				}

				AuthenticatedUserName = HttpContext.Current.User.Identity.Name;
				AuthenticatedUserIdentifier = currentUser.Identifier;
				AuthenticatedUserName = currentUser.Username;
				AuthenticatedUserFirstName = currentUser.FirstName;
				AuthenticatedUserLastName = currentUser.LastName;
				AuthenticatedUserGravatar = string.Format("http://www.gravatar.com/avatar/{0}?s=45", currentUser.Email.GetMd5());
			}
		}
	}
}