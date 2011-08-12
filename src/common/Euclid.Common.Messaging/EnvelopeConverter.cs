using System;
using Newtonsoft.Json;

namespace Euclid.Common.Messaging
{
	public class EnvelopeConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof (Envelope);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var typeName = string.Empty;
			do
			{
				reader.Read();

				// SELF this depends on the alphabetical order of the property names and should be changed

				if (reader.TokenType == JsonToken.PropertyName &&
				    string.Compare(reader.Value as string, "MessageTypeName", true) == 0)
				{
					reader.Read();
					typeName = reader.Value.ToString();
				}

				if (reader.TokenType == JsonToken.PropertyName && string.Compare(reader.Value as string, "Payload", true) == 0)
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

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException("Use JsonConvert.Serialize(object) to serialize this envelope");
		}
	}
}