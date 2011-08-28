using System;

namespace Euclid.Common.Messaging
{
	/// <summary>
	/// 	Publishes messages in concert with an IChannel and an IPublicationRegistry.
	/// </summary>
	public interface IPublisher
	{
		/// <summary>
		/// 	Publishes a message and returns the publication identifier of the associated publication record.
		/// </summary>
		/// <param name = "message">The message to publish.</param>
		/// <returns>The identifier of the publication record associated with this publication.</returns>
		Guid PublishMessage(IMessage message);
	}
}