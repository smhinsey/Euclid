using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class UpdateForum : DefaultCommand
	{
		public Guid ForumIdentifier { get; set; }
		public string Name { get; set; }
		public string UrlSlug { get; set; }
		public string UrlHostName { get; set; }
		public string Description { get; set; }
		public bool Private { get; set; }
		public bool Moderated { get; set; }
	}
}