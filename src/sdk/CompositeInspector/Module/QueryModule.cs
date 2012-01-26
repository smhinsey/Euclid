using System;
using System.IO;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Euclid.Composites;
using Nancy;

namespace CompositeInspector.Module
{
	public class QueryModule : NancyModule
	{
		private readonly ICompositeApp _compositeApp;

		private readonly IWindsorContainer _container;

		public QueryModule(ICompositeApp compositeApp, IWindsorContainer container)
			: base("composite/queries")
		{
			_compositeApp = compositeApp;
			_container = container;

			Get[""] = _ => "Query API";

			Get["/{queryName}"] = p => { return GetQueryByName((string)p.queryName); };

			Post["/{queryName}/{methodName}"] = p =>
				{
					var queryName = (string)p.queryName;
					var methodName = (string)p.methodName;

					return ExecuteQueryMethod(queryName, methodName);
				};
		}

		public Response ExecuteQueryMethod(string queryName, string methodName)
		{
			try
			{
				var form = (DynamicDictionary)Context.Request.Form;
				var argumentCount = form.Count();
				var results = _compositeApp.ExecuteQuery(queryName, methodName, argumentCount, paramName => form[paramName]);
				return Response.AsJson(results);
			}
			catch (Exception e)
			{
				return Response.AsJson(
					new { name = e.GetType().Name, message = e.Message, callStack = e.StackTrace }, HttpStatusCode.InternalServerError);
			}
		}

		public Response GetQueryByName(string queryName)
		{
			var asJson = false;
			if (queryName.EndsWith("json"))
			{
				asJson = true;
				queryName = queryName.Substring(0, queryName.Length - 5);
			}

			var query =
				_compositeApp.Queries.Where(q => q.Name.Equals(queryName, StringComparison.InvariantCultureIgnoreCase)).
					FirstOrDefault();

			if (asJson)
			{
				if (query == null)
				{
					return
						Response.AsJson(
							new
								{
									name = "Invalid QueryName Exception",
									message = string.Format("The composite {0} has no query named {1}", _compositeApp.Name, queryName)
								},
							HttpStatusCode.InternalServerError);
				}

				var json = query.GetFormatter().GetRepresentation("json");
				var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
				return Response.FromStream(ms, "application/json");
			}

			return View["Queries/view-query-methods.cshtml", queryName];
		}
	}
}