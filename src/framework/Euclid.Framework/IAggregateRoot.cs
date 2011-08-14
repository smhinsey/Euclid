using System;

namespace Euclid.Framework
{
	/// <summary>
	/// 	An aggregate root contains references to and can perform operations on a series of
	/// 	persistent domain model objects.
	/// </summary>
	public interface IAggregateRoot
	{
		Guid Identifier { get; }
	}
}