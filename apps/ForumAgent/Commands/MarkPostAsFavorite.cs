using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class MarkPostAsFavorite : DefaultCommand
	{
		public Guid ForumIdentifier { get; set; }
		public Guid PostIdentifier { get; set; }
		public Guid UserIdentifier { get; set; }
	}
}