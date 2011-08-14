using System;

namespace Euclid.Common.Storage
{
	/// <summary>
	/// 	An implementation of IModel encapsulates the persistent properties of a type that is important to an application's
	/// 	persistence model.
	/// </summary>
	public interface IModel
	{
		DateTime Created { get; set; }
		Guid Identifier { get; set; }
		DateTime Modified { get; set; }
	}
}