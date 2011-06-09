using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Euclid.Common.Extensions;
using Euclid.Common.Serialization;
using Euclid.Common.Transport;
using Newtonsoft.Json;

namespace Euclid.Common.UnitTests.Serialization
{
    public class EnvelopeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Use JsonConvert.Serialize(object) to serialize this envelope");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var typeName = string.Empty;
            do
            {
                reader.Read();

                if (reader.TokenType == JsonToken.PropertyName && reader.Value as string == "TypeName")
                {
                    reader.Read();
                    typeName = reader.Value.ToString();
                }

                if (reader.TokenType == JsonToken.PropertyName && reader.Value as string == "Message")
                {
                    reader.Read();
                    var type = Type.GetType(typeName);

                    var msg = serializer.Deserialize(reader, type);
                    return new Envelope(msg as IMessage);
                }

            } while (reader.TokenType != JsonToken.EndObject);



            reader.Read();

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Envelope);
        }
    }
    
    public class Envelope
    {
        public Envelope(IMessage message)
        {
            TypeName = message.GetType().FullName;
            Message = message;
        }

        public string TypeName { get; private set; }

        public IMessage Message { get; private set; }
    }

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

                var e = serializer.Deserialize(r) as Envelope;

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
