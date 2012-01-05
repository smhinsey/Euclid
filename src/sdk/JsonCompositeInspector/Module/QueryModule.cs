using System;
using System.IO;
using System.Linq;
using System.Text;
using Euclid.Composites;
using Nancy;

namespace JsonCompositeInspector.Module
{
	public class QueryModule : NancyModule
	{
		private readonly ICompositeApp _compositeApp;

		public QueryModule(ICompositeApp compositeApp): base("composite/queries")
		{
			_compositeApp = compositeApp;
			Get[""] = _ => "Query API";

			Get["/{queryName}"] = p =>
			                                        	{
			                                        		var queryName = (string) p.queryName;
			                                        		var asJson = false;
															if (queryName.EndsWith("json"))
															{
																asJson = true;
			                                          			queryName= queryName.Substring(0, queryName.Length - 5);
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
															else
															{
																return View["Queries/view-query-methods.cshtml", queryName];
															}
			                                        	};
		}
	}
}