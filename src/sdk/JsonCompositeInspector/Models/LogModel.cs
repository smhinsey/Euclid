using System.Collections.Generic;
using LoggingAgent.ReadModels;

namespace JsonCompositeInspector.Models
{
	public class LogModel
	{
		public IEnumerable<string> AvailableSources { get; set; }

		public IEnumerable<LogEntry> Entries { get; set; }

		public string SelectedSource { get; set; }
	}
}