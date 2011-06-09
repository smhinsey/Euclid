using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Euclid.Common.Extensions;
using Euclid.Common.Transport;
using Newtonsoft.Json;

namespace Euclid.Common.Serialization
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        public IMessage Deserialize(Stream source)
        {
            var serializer = new JsonSerializer();

            serializer.Converters.Insert(0, new EnvelopeConverter());

            var s = source.GetString(Encoding.UTF8);

            using (var sr = new StringReader(s))
            {
                var r = new JsonTextReader(sr);

                var e = serializer.Deserialize<Envelope>(r);

                if (e == null)
                {
                    throw new SerializationException("Cannot deserialize the stream to an IMessage object", new Exception(s));
                }

                return e.Message;
            }
        }

        public Stream Serialize(IMessage source)
        {
            var envelope = new Envelope(source);

            var s = JsonConvert.SerializeObject(envelope);

            return s.ToMemoryStream(Encoding.UTF8);
        }
    }
}
