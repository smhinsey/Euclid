using System;
using Nancy;

namespace CompositeInspector.Extensions
{
	public static class ExceptionExtensions
	{
		public static Response CreateResponse(this Exception e, ResponseFormat format, IResponseFormatter formatter)
		{
			// dumb ugliness b/c MSFT's xml serializer can't handle anonymous objects
			var exception = new FormattedException
			                	{
			                		name = e.GetType().Name,
			                		message = e.Message,
			                		callStack = e.StackTrace
			                	};

			Response r;
			switch (format)
			{
				case ResponseFormat.Json:
					r = formatter.AsJson(exception, HttpStatusCode.InternalServerError);
					break;
				case ResponseFormat.Xml:
					r = formatter.AsXml(exception);
					break;
				default:
					r = null;
					break;
			}

			if (r != null)
			{
				r.StatusCode = HttpStatusCode.InternalServerError;
			}

			return r;
		}
	}
}