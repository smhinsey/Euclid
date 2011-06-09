using System.IO;
using Euclid.Common.Transport;

namespace Euclid.Common.Serialization
{
	public interface IMessageSerializer
	{
		IMessage Deserialize(Stream source);
		Stream Serialize(IMessage source);
	}
}