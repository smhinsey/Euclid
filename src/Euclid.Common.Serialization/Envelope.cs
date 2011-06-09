
using Euclid.Common.Transport;

namespace Euclid.Common.Serialization
{
    public class Envelope
    {
        public Envelope(IMessage message)
        {
            TypeName = message.GetType().AssemblyQualifiedName;
            Message = message;
        }

        public string TypeName { get; private set; }

        public IMessage Message { get; private set; }
    }
}