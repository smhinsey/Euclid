using System;
using System.Web.Mvc;

namespace Euclid.Composites.Mvc.Results
{
	public class JsonpNetResult : JsonNetResult
	{
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			var request = context.HttpContext.Request;
			var response = context.HttpContext.Response;

			var jsoncallback = (context.RouteData.Values["callback"] as string) ?? request["callback"];

			if (!string.IsNullOrEmpty(jsoncallback))
			{
				response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

				response.Write(string.Format("{0}(", jsoncallback));
			}

			base.ExecuteResult(context);

			if (!string.IsNullOrEmpty(jsoncallback))
			{
				response.Write(");");
			}
		}
	}
}