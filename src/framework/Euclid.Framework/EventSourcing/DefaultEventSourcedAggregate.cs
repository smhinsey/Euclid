using System;

namespace Euclid.Framework.EventSourcing
{
	public class DefaultEventSourcedAggregate : IEventSourcedAggregate
	{
		public IEvent CurrentAsOf { get; protected set; }

		public DateTime EventLastApplied { get; protected set; }

		public Guid Identifier { get; protected set; }
	}
}