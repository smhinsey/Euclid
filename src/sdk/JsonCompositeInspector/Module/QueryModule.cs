using Nancy;

namespace JsonCompositeInspector.Module
{
	public class QueryModule : NancyModule
	{
		public QueryModule(): base("composite/queries")
		{
			Get[""] = _ => "Query API";
		}
	}
}