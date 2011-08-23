using System;

namespace Euclid.Common.Messaging
{
	/// <summary>
	/// 	The fundamental contract for a message.
	/// </summary>
	public interface IMessage
	{
		/// <summary>
		/// Gets or sets the date and time when the message was first published.
		/// </summary>
		DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the user who created the message.
		/// </summary>
		Guid CreatedBy { get; set; }

		/// <summary>
		/// Gets or sets a message's unique identifier.
		/// </summary>
		Guid Identifier { get; set; }
	}
}