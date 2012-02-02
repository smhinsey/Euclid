using System;
using System.IO;
using System.Linq;
using System.Text;
using Euclid.Framework.AgentMetadata;
using Nancy;

namespace CompositeInspector.Extensions
{
	public static class FormatExtensions
	{
		public static ResponseFormat GetResponseFormat(this NancyModule module)
		{
			return module.Context.GetResponseFormat();
		}

		public static ResponseFormat GetResponseFormat(this NancyContext ctx)
		{
			var format = ResponseFormat.Html;
			if (
				ctx.Request.Headers.Accept.Any(a => a.IndexOf("application/json", StringComparison.CurrentCultureIgnoreCase) >= 0)
				||
				ctx.Request.Path.EndsWith(".json", StringComparison.CurrentCultureIgnoreCase)
				)
			{
				format = ResponseFormat.Json;
			}
			else if (
				ctx.Request.Headers.Accept.Any(a => a.IndexOf("text/xml", StringComparison.CurrentCultureIgnoreCase) >= 0)
				||
				ctx.Request.Headers.Accept.Any(a => a.IndexOf("application/xml", StringComparison.CurrentCultureIgnoreCase) >= 0)
				||
				ctx.Request.Path.EndsWith(".xml", StringComparison.CurrentCultureIgnoreCase)
				)
			{
				format = ResponseFormat.Xml;
			}

			return format;
		}

		public static Response WriteTo(this IMetadataFormatter formatter, IResponseFormatter response)
		{
			var format = response.Context.GetResponseFormat();

			var representation = format == ResponseFormat.Json ? "json" : "xml";
			var encodedString = formatter.GetRepresentation(representation);
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(encodedString));
			return response.FromStream(stream, Euclid.Common.Extensions.MimeTypes.GetByExtension(representation));
		}
	}
}