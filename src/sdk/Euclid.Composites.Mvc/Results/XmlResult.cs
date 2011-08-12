using System;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using Euclid.Common.Extensions;

namespace Euclid.Composites.Mvc.Results
{
	public class XmlResult : ActionResult
	{
		public object Data { get; set; }

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			var response = context.HttpContext.Response;

			response.ContentEncoding = Encoding.UTF8;
			response.ContentType = MimeTypes.GetByExtension("xml");

			if (Data != null)
			{
				var root = new XElement(Data.GetType().Name);

				foreach (var property in Data.GetType().GetProperties())
				{
					var rawValue = property.GetValue(Data, null) ?? string.Empty;

					var value = (property.PropertyType == typeof (Type))
					            	? (rawValue as Type).FullName
					            	: rawValue.ToString();

					root.Add(new XElement(property.Name, value));
				}

				response.Output.Write(root.ToString());
			}
		}
	}
}