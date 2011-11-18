using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class ApprovePost : DefaultCommand
	{
		public Guid PostIdentifier { get; set; }
		public Guid ApprovedBy { get; set; }
	}
}