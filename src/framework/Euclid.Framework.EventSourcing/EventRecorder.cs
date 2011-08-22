using System;
using Euclid.Framework.Cqrs;
using EventStore;

namespace Euclid.Framework.EventSourcing
{
	/// <summary>
	/// 	The EventRecorder is responsible for persisting IEvents within an EventStore stream.
	/// </summary>
	public class EventRecorder : DefaultCommandProcessor<IEvent>
	{
		private readonly IStoreEvents _eventStore;

		private readonly Guid _streamId;

		public EventRecorder(IStoreEvents eventStore, Guid streamId)
		{
			this._eventStore = eventStore;
			this._streamId = streamId;
		}

		public override void Process(IEvent message)
		{
			using (var stream = this._eventStore.CreateStream(this._streamId))
			{
				stream.Add(new EventMessage { Body = message });
			}
		}
	}
}