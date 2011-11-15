using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using LoggingAgent.ReadModels;
using NHibernate;

namespace LoggingAgent.Queries
{
	public class LogQueries : NhQuery<LogEntry>
	{
		public LogQueries(ISession session)
			: base(session)
		{
		}

		public IEnumerable<LogEntry> GetLogEntries(int pageSize, int offset)
		{
			return GetCurrentSession().QueryOver<LogEntry>().Skip(offset * pageSize).Take(pageSize).List();
		}
	}
}