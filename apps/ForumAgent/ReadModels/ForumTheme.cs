using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class ForumTheme : DefaultReadModel
	{
		public virtual bool IsCurrent { get; set; }
		public virtual string Name { get; set; }
		public virtual string PreviewUrl { get; set; }
	}
}
