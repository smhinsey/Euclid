using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Euclid.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Euclid.Framework.AgentMetadata.Formatters
{
	public abstract class MetadataFormatter : IMetadataFormatter
	{
		private readonly IDictionary<string, string> _supportedContentTypes = new Dictionary<string, string>
		                                                                      	{
		                                                                      		{"xml", MimeTypes.GetByExtension("xml")},
		                                                                      		{"json", MimeTypes.GetByExtension("json")}
		                                                                      	};

		public string GetContentType(string format)
		{
			return !_supportedContentTypes.ContainsKey(format) ? null : _supportedContentTypes[format];
		}

		public Encoding GetEncoding(string format)
		{
			return Encoding.UTF8;
		}

		public IEnumerable<string> GetFormats(string contentType)
		{
			return _supportedContentTypes.Where(item => item.Value == contentType).Select(item => item.Key);
		}

		public string GetRepresentation(string format)
		{
			switch (format.ToLower())
			{
				case "xml":
					return GetAsXml();
				case "json":
					return SetupJsonSerialization();
			}

			throw new MetadataFormatNotSupportedException(format);
		}

		public string SetupJsonSerialization()
		{
			var serializerSettings = new JsonSerializerSettings();

			serializerSettings.Converters.Add(new IsoDateTimeConverter());

			var json = new StringBuilder();

			var writer = new JsonTextWriter(new StringWriter(json)) {Formatting = Formatting.Indented};

			var serializer = JsonSerializer.Create(serializerSettings);

			var data = GetJsonObject(serializer);

			serializer.Serialize(writer, data);

			return json.ToString();
		}

		protected abstract string GetAsXml();
		protected abstract object GetJsonObject(JsonSerializer serializer);
	}
}