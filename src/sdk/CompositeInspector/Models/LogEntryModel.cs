using System.Collections.Generic;
using System.Linq;
using LoggingAgent.ReadModels;

namespace CompositeInspector.Models
{
	public class LogEntryModel
	{
		public IEnumerable<LogEntry> Entries { get; set; }
		public string SelectedSource { get; set; }
		public IEnumerable<string> AvailableSources { get; set; }
	}
}