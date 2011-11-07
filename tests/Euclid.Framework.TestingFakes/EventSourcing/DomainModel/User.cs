﻿using System.Collections.Generic;
using Euclid.Framework.EventSourcing;

namespace Euclid.Framework.TestingFakes.EventSourcing.DomainModel
{
	public class User : DefaultEventSourcedAggregate
	{
		public virtual string Username { get; set; }
		public virtual IList<Post> Posts { get; set; }
	}
}