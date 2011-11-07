using System;
using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class Forum : DefaultReadModel
	{
		public virtual string Name { get; set; }

		public virtual string UrlHostName { get; set; }

		public virtual string UrlSlug { get; set; }

		//public virtual IList<Guid> ForumAdminIdentifiers { get; set; }

		//public virtual IList<Guid> ModeratorIdentifiers { get; set; }

		//public virtual IList<Guid> MemberIdentifiers { get; set; }
	}
}