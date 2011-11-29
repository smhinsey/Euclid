using System;
using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class AvailableContent : SyntheticReadModel
	{
		public Guid ForumIdentifier { get; set; }
		public string ForumName { get; set; }
		public int TotalContentItems { get; set; }
		public IList<ForumContent> ContentItems { get; set; }
	}
}