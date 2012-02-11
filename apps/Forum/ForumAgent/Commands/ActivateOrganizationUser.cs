using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class ActivateOrganizationUser : DefaultCommand
	{
		public Guid UserIdentifier { get; set; }
		public bool Active { get; set; }
	}
}