using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class SetForumTheme : DefaultCommand
	{
		public Guid ForumIdentifier { get; set; }
		public string ThemeName { get; set; }
	}
}