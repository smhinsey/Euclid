namespace Euclid.Framework.EventSourcing
{
	/// <summary>
	/// 	An event-sourced aggregate is an aggregate root which is explicitly managed
	/// 	by event-sourcing, or the application of a series of changes to itself 
	/// 	encapsulated in IEvent instances.
	/// </summary>
	public interface IEventSourcedAggregate : IAggregateRoot
	{
		IEvent CurrentAsOf { get; }
	}
}