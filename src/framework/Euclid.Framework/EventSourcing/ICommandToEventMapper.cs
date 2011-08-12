using System.Collections.Generic;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.EventSourcing
{
	/// <summary>
	/// 	A command-to-event mapper converts a CQRS ICommand into one or more
	/// 	instances of IEvent types.
	/// </summary>
	public interface ICommandToEventMapper<in TCommand>
		where TCommand : ICommand
	{
		IList<IEvent> Convert(TCommand command);
	}
}