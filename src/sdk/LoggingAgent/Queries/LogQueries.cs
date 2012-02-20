using System;
using System.Collections.Generic;
using Euclid.Common.Messaging;
using Euclid.Framework.Cqrs;
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

		public LogEntries GetLogEntries(int pageSize, int offset)
		{
			var session = GetCurrentSession();
			var totalRecords = session.QueryOver<CommandPublicationRecord>().RowCount();
			var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
			var currentPage = offset > totalPages * pageSize ? totalPages : offset / pageSize + 1;

			return new LogEntries
					{
						Entries = session.QueryOver<LogEntry>().Skip(offset * pageSize).Take(pageSize).List(),
						TotalRecords = totalRecords,
						TotalPages = totalPages,
						CurrentPage = currentPage,
						PreviousPage = currentPage > 1 ? currentPage - 1 : 1,
						NextPage = currentPage < totalPages ? currentPage + 1 : totalPages,
						Offset = offset,
						RecordsPerPage = pageSize,
						Created = DateTime.Now,
						Identifier = Guid.Empty,
						Modified = DateTime.Now
					};
		}
	}
}