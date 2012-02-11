using System;
using System.Collections.Generic;

namespace ForumAgent.ReadModels
{
	public class AvailableTags
	{
		public string ForumName { get; set; }
		public Guid ForumIdentifier { get; set; }
		public IList<Tag> Tags { get; set; }
		public int TotalTags { get; set; }
	}
}