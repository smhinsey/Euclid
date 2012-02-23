using System;
using Euclid.Composites;
using LoggingAgent.Queries;
using Nancy;
using Nancy.Responses;

namespace CompositeInspector.Module
{
	public class UserInterfaceModule : NancyModule
	{
		private const string BaseRoute = "composite";

		private const string IndexRoute = "";
		private const string HomeRoute = "/inspector";
		private const string HomeViewPath = "inspector";

		public UserInterfaceModule()
			: base(BaseRoute)
		{
			Get[IndexRoute] = _ => View[HomeViewPath];

			Get[HomeRoute] = _ => View[HomeViewPath];
		}
	}
}