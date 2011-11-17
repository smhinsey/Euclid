using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class ActivateContent : DefaultCommand
	{
		public Guid ContentIdentifier { get; set; }
		public bool Active { get; set; }
	}
}