using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Euclid.Composites;
using JsonCompositeInspector.Views.Commands.Binders;
using Nancy;

namespace JsonCompositeInspector.Module
{
	public class QueryModule : NancyModule
	{
		private readonly ICompositeApp _compositeApp;
		private readonly IWindsorContainer _container;

		public QueryModule(ICompositeApp compositeApp, IWindsorContainer container): base("composite/queries")
		{
			_compositeApp = compositeApp;
			_container = container;

			Get[""] = _ => "Query API";

			Get["/{queryName}"] = p =>
									{
										return GetQueryByName((string) p.queryName);
									};

			Post["/{queryName}/{methodName}"] = p =>
													{
														var queryName = (string)p.queryName;
														var methodName = (string)p.methodName;

														return ExecuteQueryMethod(queryName, methodName);
													};
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
				_compositeApp.Queries.Where(
					q =>
					q.Name.Equals(queryName, StringComparison.InvariantCultureIgnoreCase)).
					FirstOrDefault();

			if (asJson)
			{
				var json = query.GetFormatter().GetRepresentation("json");
				var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
				return Response.FromStream(ms, "application/json");
			}

			return View["Queries/view-query-methods.cshtml", queryName];
		}

		public Response ExecuteQueryMethod(string queryName, string methodName)
		{
			var query = _compositeApp.Queries.Where(
				q =>
				q.Name.Equals(queryName, StringComparison.InvariantCultureIgnoreCase)).
				FirstOrDefault();

			var instance = _container.Resolve(query.Type);
			var arguments = new List<object>();
			var argumentCount = ((DynamicDictionary)Context.Request.Form).Count();
			var method = query.Type.GetMethods().Where(m => m.Name == methodName && m.GetParameters().Count() == argumentCount).FirstOrDefault();
			try
			{
				foreach (var param in method.GetParameters())
				{
					var value = Context.Request.Form[param.Name];
					arguments.Add(ValueConverter.GetValueAs(value, param.ParameterType));
				}

				var results = method.Invoke(instance, arguments.ToArray());
				return Response.AsJson(results);
			}
			catch (Exception e)
			{
				return Response.AsJson(new { name = e.GetType().Name, message = e.Message, callStack = e.StackTrace }, HttpStatusCode.InternalServerError);
			}

		}
	}
}