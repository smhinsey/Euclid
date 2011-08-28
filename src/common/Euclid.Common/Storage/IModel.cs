using System;

namespace Euclid.Common.Storage
{
	/// <summary>
	/// 	An implementation of IModel encapsulates the persistent properties of a type that is important to an application's
	/// 	persistence model.
	/// </summary>
	public interface IModel
	{
		/// <summary>
		/// 	Gets or sets the date the model was created.
		/// </summary>
		DateTime Created { get; set; }

		/// <summary>
		/// 	Gets or sets the model's unique identifier.
		/// </summary>
		Guid Identifier { get; set; }

		/// <summary>
		/// 	Gets or sets the date of the last time the model was modified.
		/// </summary>
		DateTime Modified { get; set; }
	}
}