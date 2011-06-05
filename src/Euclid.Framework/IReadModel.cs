using System;

namespace Euclid.Framework.EventSourcing
{
	/// <summary>
	/// The basic contract for defining a read model, or a view of an aggregate which is optimized
	/// for use in a composite user interface such as an MVC application.
	/// </summary>
	public interface IReadModel
	{
		Guid Identifier { get; }
	}
}