using System.IO;
using Euclid.Common.Transport;

namespace Euclid.Common.Serialization
{
    public interface ITypeSerializer<T>
    {
        T Deserialize(Stream source);
        Stream Serialize(T source);
    }

    public interface IMessageSerializer : ITypeSerializer<IMessage>
    {
    }
}
