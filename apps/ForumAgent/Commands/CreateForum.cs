using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class CreateForum : DefaultCommand
	{
		public string Description { get; set; }

		public string Name { get; set; }

		public Guid OrganizationId { get; set; }

		public string UrlHostName { get; set; }

		public string UrlSlug { get; set; }

		public bool UpDownVoting { get; set; }

		public string Theme { get; set; }

		public bool Moderated { get; set; }

		public bool Private { get; set; }
	}
}