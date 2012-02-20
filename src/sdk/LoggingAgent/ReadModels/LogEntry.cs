using System;
using System.Collections.Generic;
using Euclid.Framework.Models;

namespace LoggingAgent.ReadModels
{
	public class LogEntry : DefaultReadModel
	{
		public virtual DateTime Date { get; set; }

		public virtual string Exception { get; set; }

		public virtual int Id { get; set; }

		public virtual string Level { get; set; }

		public virtual string Logger { get; set; }

		public virtual string LoggingSource { get; set; }

		public virtual string Message { get; set; }

		public virtual string Thread { get; set; }
	}

	public class LogEntries : SyntheticReadModel
	{
		public int Offset { get; set; }
		public int RecordsPerPage { get; set; }
		public int TotalRecords { get; set; }
		public int TotalPages { get; set; }
		public int CurrentPage;
		public int NextPage { get; set; }
		public int PreviousPage { get; set; }
		public IEnumerable<LogEntry> Entries { get; set; }
	}
}