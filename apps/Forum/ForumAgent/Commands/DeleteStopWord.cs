using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class DeleteStopWord : DefaultCommand
	{
		public Guid StopWordIdentifier { get; set; }
	}
}