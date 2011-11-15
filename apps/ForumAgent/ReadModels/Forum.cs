﻿using System;
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
	}
}