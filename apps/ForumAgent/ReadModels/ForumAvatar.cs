using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class ForumAvatar : DefaultReadModel
	{
		public virtual Guid ForumIdentifier { get; set; }
		public virtual string Name { get; set; }
		public virtual string Url { get; set; }
		public virtual string Description { get; set; }
		public virtual bool Active { get; set; }
		public virtual Guid CreatedBy { get; set; }
	}
}