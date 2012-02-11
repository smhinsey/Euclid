using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class ForumContent : DefaultReadModel
	{
		public virtual bool Active { get; set; }
		public virtual Guid ForumIdentifier { get; set; }
		public virtual string ContentType { get; set; }
		public virtual string ContentLocation { get; set; }
		public virtual Guid CreatedBy { get; set; }
		public virtual string Value { get; set; }
	}
}