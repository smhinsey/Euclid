using System.Collections.Generic;
using System.Linq;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Queries
{
	public class ForumHostQueries : IQuery
	{
		public List<string> GetHosts()
		{
			return System.Configuration.ConfigurationManager.AppSettings["AdminComposite.AvailableHosts"].Split(new[] {';'}).ToList();
		}
	}
}
