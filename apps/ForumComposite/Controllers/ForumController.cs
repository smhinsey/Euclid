﻿using System.Web.Mvc;
using System.Web.Routing;
using Euclid.Common.Messaging;

namespace ForumComposite.Controllers
{
	public abstract class ForumController : Controller
	{
		protected ForumController()
		{
			Publisher = DependencyResolver.Current.GetService<IPublisher>();
		}

		public SharedForumInfo ForumInfo { get; private set; }

		public IPublisher Publisher { get; private set; }

		protected override void Execute(RequestContext requestContext)
		{
			var descriptor = new SharedForumInfo();

			descriptor.Initialize(requestContext.RouteData);

			ForumInfo = descriptor;

			base.Execute(requestContext);
		}
	}
}