using System;
using Euclid.Framework.Models;

namespace Euclid.Framework.EventSourcing
{
	/// <summary>
	/// 	An event-sourced aggregate is an aggregate root which is explicitly managed
	/// 	by event-sourcing, or the application of a series of changes to itself 
	/// 	encapsulated in IEvent instances.
	/// </summary>
	public interface IEventSourcedAggregate : IAggregateRoot
	{
		/// <summary>
		/// 	Gets the last event that updated the aggregate.
		/// </summary>
		IEvent CurrentAsOf { get; }

		/// <summary>
		/// 	Gets the time of the last modification to the aggregate.
		/// </summary>
		DateTime EventLastApplied { get; }
	}
}