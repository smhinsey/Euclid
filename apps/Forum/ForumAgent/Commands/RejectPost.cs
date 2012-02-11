using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class RejectPost : DefaultCommand
	{
		public Guid PostIdentifier { get; set; }
	}
}