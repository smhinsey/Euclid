using System;
using Euclid.Common.Transport;
using Newtonsoft.Json;

namespace Euclid.Common.Serialization
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

                if (reader.TokenType == JsonToken.PropertyName && string.Compare(reader.Value as string, "TypeName", true) == 0)
                {
                    reader.Read();
                    typeName = reader.Value.ToString();
                }

                if (reader.TokenType == JsonToken.PropertyName && string.Compare(reader.Value as string, "Message", true) == 0)
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
}