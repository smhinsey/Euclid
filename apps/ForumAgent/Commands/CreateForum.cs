using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class CreateForum : DefaultCommand
	{
		public string Name { get; set; }

		public string UrlHostName { get; set; }

		public string UrlSlug { get; set; }

		public string Description { get; set; }

		public Guid OrganizationId { get; set; }

		public bool UpDownVoting { get; set; }
	}
}