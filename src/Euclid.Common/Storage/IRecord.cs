using System;

namespace Euclid.Common.Storage
{
	/// <summary>
	/// 	A small piece of information which needs to be persisted to an arbitrary medium.
	/// </summary>
	public interface IRecord
	{
		DateTime Created { get; }
		Guid Identifier { get; }
		DateTime Modified { get; }
	}
}