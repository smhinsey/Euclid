using System;

namespace Euclid.Common.Storage
{
	/// <summary>
	/// A small piece of information which needs to be persisted to an arbitrary medium.
	/// </summary>
	public interface IRecord
	{
		Guid Identifier { get; }
		DateTime Created { get; }
		DateTime Modified { get; }
	}
}