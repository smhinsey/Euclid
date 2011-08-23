using System;

namespace Euclid.Common.Messaging
{
	/// <summary>
	/// Contains data pertaining to the publication of a message.
	/// </summary>
	public interface IPublicationRecord : IMessage
	{
		/// <summary>
		/// Gets or sets the CallStack of an exception associated with the message.
		/// </summary>
		string CallStack { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether processing is complete for the associated message. If so,
		/// no further work needs to be done and all subscribed processors have executed without error.
		/// </summary>
		bool Completed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the associated message has been dispatched to a processor.
		/// </summary>
		bool Dispatched { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether an exception or other error has occurred during any part
		/// of the associated message's publication or dispatch.
		/// </summary>
		bool Error { get; set; }

		/// <summary>
		/// Gets or sets an error message, if an error has ocurred.
		/// </summary>
		string ErrorMessage { get; set; }

		/// <summary>
		/// Gets or sets the location where the message associated with this record is stored.
		/// </summary>
		Uri MessageLocation { get; set; }

		/// <summary>
		/// Gets or sets the concrete type of the message associated with this record.
		/// </summary>
		Type MessageType { get; set; }
	}
}