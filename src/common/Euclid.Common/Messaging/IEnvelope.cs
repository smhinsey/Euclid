namespace Euclid.Common.Messaging
{
	public interface IEnvelope : IMessage
	{
		string MessageTypeName { get; }
		IMessage Payload { get; set; }
	}
}