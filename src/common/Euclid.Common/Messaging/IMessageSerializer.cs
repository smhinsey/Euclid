namespace Euclid.Common.Messaging
{
	/// <summary>
	/// 	Responsible for serializing messages.
	/// </summary>
	public interface IMessageSerializer
	{
		/// <summary>
		/// 	Deserializes a message.
		/// </summary>
		/// <param name = "source">The serialized message.</param>
		/// <returns>The deserialized message.</returns>
		IMessage Deserialize(byte[] source);

		/// <summary>
		/// 	Serializes a message.
		/// </summary>
		/// <param name = "source">The message to be serialized.</param>
		/// <returns>The serialized message.</returns>
		byte[] Serialize(IMessage source);
	}
}
