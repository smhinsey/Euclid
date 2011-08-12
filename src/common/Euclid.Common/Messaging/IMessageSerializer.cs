namespace Euclid.Common.Messaging
{
	public interface IMessageSerializer
	{
		IMessage Deserialize(byte[] source);
		byte[] Serialize(IMessage source);
	}
}