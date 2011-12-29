using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Euclid.Common.Messaging;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace ForumComposite.Controllers
{
	// TODO: all of the below queries need to reside elsewhere
	// TODO: all of the below queries need to be combined into a single one
	public abstract class ForumController : Controller
	{
		private readonly ForumQueries _forumQueries;
		private readonly OrganizationQueries _orgQueries;
		private readonly CategoryQueries _categoryQueries;
		private readonly UserQueries _userQueries;

		protected ForumController()
		{
			_forumQueries = DependencyResolver.Current.GetService<ForumQueries>();
			_orgQueries = DependencyResolver.Current.GetService<OrganizationQueries>();
			_categoryQueries = DependencyResolver.Current.GetService<CategoryQueries>();
			_userQueries = DependencyResolver.Current.GetService<UserQueries>();
			Publisher = DependencyResolver.Current.GetService<IPublisher>();
		}

		protected override void Execute(System.Web.Routing.RequestContext requestContext)
		{
			setGlobalProperties(requestContext);

			base.Execute(requestContext);
		}

		public Guid ForumIdentifier { get; set; }
		public string ForumName { get; set; }
		public Guid OrganizationIdentifier { get; set; }
		public string OrganizationName { get; set; }
		public IList<Category> Categories { get; set; }
		public IList<ForumUser> TopUsers { get; set; }

		public string CurrentUserName { get; set; }

		public IPublisher Publisher { get; set; }

		private void setGlobalProperties(RequestContext requestContext)
		{
			var orgSlug = requestContext.RouteData.Values["org"].ToString();
			var forumSlug = requestContext.RouteData.Values["forum"].ToString();

			var org = _orgQueries.FindBySlug(orgSlug);
			var forum = _forumQueries.FindBySlug(org.Identifier, forumSlug);

			OrganizationIdentifier = org.Identifier;
			OrganizationName = org.Name;

			ForumName = forum.Name;
			ForumIdentifier = forum.Identifier;

			Categories = _categoryQueries.GetActiveCategories(ForumIdentifier, 0, 100);
			TopUsers = _userQueries.FindTopUsers(ForumIdentifier);

			if (requestContext.HttpContext.Request.IsAuthenticated)
			{
				CurrentUserName = requestContext.HttpContext.User.Identity.Name;
			}
		}
	}
}