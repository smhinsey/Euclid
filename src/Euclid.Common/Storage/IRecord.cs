using System;

namespace Euclid.Common.Storage
{
	public interface IRecord
	{
		Guid Identifier { get; }
		DateTime Created { get; }
		DateTime Modified { get; }
	}
}