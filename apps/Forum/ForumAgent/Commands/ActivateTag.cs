using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class ActivateTag : DefaultCommand
	{
		public Guid TagIdentifier { get; set; }
		public bool Active { get; set; }
	}
}