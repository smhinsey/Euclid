using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Euclid.Common.Extensions;
using Euclid.Common.Transport;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

				return e.Payload;
			}
		}

		public Stream Serialize(IMessage source)
		{
			var envelope = new Envelope(source);

			var settings = new JsonSerializerSettings
			               	{
			               		ContractResolver = new CamelCasePropertyNamesContractResolver()
			               	};

			var s = JsonConvert.SerializeObject(envelope, Formatting.None, settings);

			return s.ToMemoryStream(Encoding.UTF8);
		}
	}
}