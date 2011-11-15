﻿using System.Collections.Generic;
using LoggingAgent.ReadModels;

namespace CompositeInspector.Models
{
	public class LogEntryModel
	{
		public IEnumerable<string> AvailableSources { get; set; }

		public IEnumerable<LogEntry> Entries { get; set; }

		public string SelectedSource { get; set; }
	}
}