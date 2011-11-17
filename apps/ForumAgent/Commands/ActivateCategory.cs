using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class ActivateCategory : DefaultCommand
	{
		public Guid CategoryIdentifier { get; set; }
		public bool Active { get; set; }
	}
}