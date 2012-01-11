using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class StopWord : DefaultReadModel
	{
		public virtual Guid	 ForumIdentifier { get; set; }
		public virtual string WordToMatch { get; set; }
		public virtual string ReplacementWord { get; set; }
		public virtual bool Active { get; set; } 
	}
}