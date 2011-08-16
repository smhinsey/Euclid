using System;
using Euclid.Framework.Cqrs;
using EventStore;

namespace Euclid.Framework.EventSourcing
{
	public class EventRecorder : DefaultCommandProcessor<IEvent>
	{
		private readonly IStoreEvents _eventStore;
		private readonly Guid _streamId;

		public EventRecorder(IStoreEvents eventStore, Guid streamId)
		{
			_eventStore = eventStore;
			_streamId = streamId;
		}

		public override void Process(IEvent message)
		{
			using(var stream = _eventStore.CreateStream(_streamId))
			{
				stream.Add(new EventMessage {Body = message});
			}
		}
	}
}