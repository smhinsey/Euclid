namespace Euclid.Common.Messaging
{
	public class Envelope
	{
		public Envelope(IMessage message)
		{
			MessageTypeName = message.GetType().AssemblyQualifiedName;
			Payload = message;
		}

		public string MessageTypeName { get; private set; }
		public IMessage Payload { get; private set; }
	}
}