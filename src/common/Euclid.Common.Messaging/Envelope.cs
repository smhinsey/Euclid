namespace Euclid.Common.Messaging
{
	public class Envelope
	{
		public Envelope(IMessage message)
		{
			this.MessageTypeName = message.GetType().AssemblyQualifiedName;
			this.Payload = message;
		}

		public string MessageTypeName { get; private set; }

		public IMessage Payload { get; private set; }
	}
}