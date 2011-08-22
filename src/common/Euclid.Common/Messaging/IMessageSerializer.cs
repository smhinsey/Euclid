namespace Euclid.Common.Messaging
{
	/// <summary>
	/// 	Responsible for serializing messages.
	/// </summary>
	public interface IMessageSerializer
	{
		IMessage Deserialize(byte[] source);

		byte[] Serialize(IMessage source);
	}
}