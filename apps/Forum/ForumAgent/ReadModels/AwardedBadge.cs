using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class AwardedBadge : DefaultReadModel
	{
		public virtual Guid BadgeIdentifier { get; set; }
		public virtual Guid UserIdentifier { get; set; }
		public virtual Guid ForumIdentifier { get; set; }
	}
}