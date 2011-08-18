using Euclid.Common.Storage;

namespace Euclid.Common.Messaging
{
	public class DefaultEnvelope : DefaultRecord, IEnvelope
	{
		public DefaultEnvelope()
		{
		}

		public DefaultEnvelope(IMessage message)
		{
			MessageTypeName = message.GetType().AssemblyQualifiedName;
			Payload = message;
		}

		public virtual string MessageTypeName { get; set; }
		public virtual IMessage Payload { get; set; }
	}
}