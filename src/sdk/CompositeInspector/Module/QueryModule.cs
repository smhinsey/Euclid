﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using Euclid.Composites;
using Nancy;

namespace CompositeInspector.Module
{
	public class QueryModule : NancyModule
	{
		private const string BasePath = "composite/queries";

		private const string ExecuteQueryRoute = "/{queryName}/{methodName}";

		private const string IndexRoute = "";

		private const string QueryMetadataRoute = "/{queryName}";

		private const string QueryMetadataViewPath = "Queries/view-query-metadata.cshtml";

		private readonly ICompositeApp _compositeApp;

		public QueryModule(ICompositeApp compositeApp)
			: base(BasePath)
		{
			_compositeApp = compositeApp;

			Get[IndexRoute] = _ => "Query API";

			//Get[QueryMetadataRoute] = p => GetQueryMetadataByName((string)p.queryName);

			Post[ExecuteQueryRoute] = p =>
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
				var model = new { name = e.GetType().Name, message = e.Message, callStack = e.StackTrace };

				return Response.AsJson(model, HttpStatusCode.InternalServerError);
			}
		}
	}
}