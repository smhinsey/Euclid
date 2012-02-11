using System;
using System.Collections.Generic;

namespace ForumAgent.ReadModels
{
	public class AvailableStopWords
	{
		public string ForumName { get; set; }
		public Guid ForumIdentifier { get; set; }
		public IList<StopWord> StopWords { get; set; }
		public int TotalStopWords { get; set; }
	}
}