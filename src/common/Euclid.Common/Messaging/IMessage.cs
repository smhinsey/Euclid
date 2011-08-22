using System;

namespace Euclid.Common.Messaging
{
	/// <summary>
	/// 	The fundamental contract for a message.
	/// </summary>
	public interface IMessage
	{
		/// <summary>
		/// 	Indicates when the message was created.
		/// </summary>
		DateTime Created { get; set; }

		/// <summary>
		/// 	Indicates which user created the message.
		/// </summary>
		Guid CreatedBy { get; set; }

		/// <summary>
		/// 	A unique identifier for the message.
		/// </summary>
		Guid Identifier { get; set; }
	}
}