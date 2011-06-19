using System.IO;
using Euclid.Common.Transport;

namespace Euclid.Common.Serialization
{
	public interface IMessageSerializer
	{
		IMessage Deserialize(byte[] source);
		byte[] Serialize(IMessage source);
	}
}