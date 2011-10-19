using System;

namespace Euclid.Framework.EventSourcing
{
	public abstract class DefaultEvent : IEvent
	{
		public DateTime Created { get; set; }

		public Guid CreatedBy { get; set; }

		public Guid Identifier { get; set; }

		public DateTime TriggeredAt { get; protected set; }
	}
}