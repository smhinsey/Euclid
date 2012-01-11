using Euclid.Composites;
using LoggingAgent.Queries;
using Nancy;

namespace JsonCompositeInspector.Module
{
	public class LogModule : NancyModule
	{
		private readonly ICompositeApp _compositeApp;
		private readonly LogQueries _logQueries;

		public LogModule(ICompositeApp compositeApp, LogQueries logQueries) : base("composite/logs")
		{
			_compositeApp = compositeApp;
			_logQueries = logQueries;
		}
	}
}