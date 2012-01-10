using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class CreateStopWord : DefaultCommand
	{
		public Guid ForumIdentifier { get; set; }
		public bool Active { get; set; }
		public string WordToMatch { get; set; }
		public string ReplacementWord { get; set; }
	}
}