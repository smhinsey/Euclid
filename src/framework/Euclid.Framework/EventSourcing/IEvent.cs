using System;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.EventSourcing
{
	/// <summary>
	/// 	An event encapsulates the consequences of a modification to the state of the system.
	/// </summary>
	public interface IEvent : ICommand
	{
		/// <summary>
		/// Gets the time when the event was triggered.
		/// </summary>
		DateTime TriggeredAt { get; }
	}
}