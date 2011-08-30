using System;
using Euclid.Framework.Models;

namespace LoggingAgent.ReadModels
{
	public class LogEntry : DefaultReadModel
	{
		public virtual int Id { get; set; }
		public virtual DateTime Date { get; set; }
		public virtual string Thread { get; set; }
		public virtual string Level { get; set; }
		public virtual string Logger { get; set; }
		public virtual string Message { get; set; }
		public virtual string Exception { get; set; }
		public virtual string LoggingSource { get; set; }
	}
}