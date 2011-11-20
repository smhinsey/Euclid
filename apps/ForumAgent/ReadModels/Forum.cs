using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class Forum : DefaultReadModel
	{
		public virtual string Description { get; set; }

		public virtual string Name { get; set; }

		public virtual Guid OrganizationId { get; set; }

		public virtual string UrlHostName { get; set; }

		public virtual string UrlSlug { get; set; }

		public virtual int TotalPosts { get; set; }

		public virtual bool NoVoting { get; set; }

		public virtual bool UpDownVoting { get; set; }

		public virtual Guid CreatedBy { get; set; }

		public virtual string Theme { get; set; }

		public virtual bool Moderated { get; set; }

		public virtual bool Private { get; set; }
	}
}