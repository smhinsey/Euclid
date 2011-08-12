using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Euclid.Composites.Mvc.Results
{
	/// <summary>
	/// 	A result container which serializes its content to JSON using the JSON.NET serializer. This is useful because unlike
	/// 	the built-in JSON serializer, this one supports dates in a standard format resulting in easier interop with client-side script.
	/// </summary>
	public class JsonNetResult : ActionResult
	{
		public JsonNetResult()
		{
			SerializerSettings = new JsonSerializerSettings();

			SerializerSettings.Converters.Add(new IsoDateTimeConverter());
		}

		public Encoding ContentEncoding { get; set; }
		public string ContentType { get; set; }
		public object Data { get; set; }

		public Formatting Formatting { get; set; }
		public JsonSerializerSettings SerializerSettings { get; set; }

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			var response = context.HttpContext.Response;

			response.ContentType = !string.IsNullOrEmpty(ContentType)
			                       	? ContentType
			                       	: "application/json";

			if (ContentEncoding != null)
				response.ContentEncoding = ContentEncoding;

			if (Data != null)
			{
				var writer = new JsonTextWriter(response.Output) {Formatting = Formatting};

				var serializer = JsonSerializer.Create(SerializerSettings);
				serializer.Serialize(writer, Data);

				writer.Flush();
			}
		}
	}
}