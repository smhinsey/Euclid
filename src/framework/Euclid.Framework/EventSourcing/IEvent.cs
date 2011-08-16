using System;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.EventSourcing
{
	/// <summary>
	/// 	An event encapsulates the consequences of a modification to the state of the system.
	/// </summary>
	public interface IEvent : ICommand
	{
		DateTime TriggeredAt { get; }
	}
}