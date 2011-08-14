using System.Web.Mvc;
using Euclid.Composites.Mvc.Results;

namespace Euclid.Composites.Mvc.ActionFilters
{
    internal class ResultsFormatter
    {
        public static ActionResult FormatActionResult(object data, string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                return null;
            }

            ActionResult result = null;
            if (!string.IsNullOrEmpty(format))
            {
                switch (format.ToLower())
                {
                    case "xml":
                        result = new XmlResult { Data = data };
                        break;
                    case "json":
                        result = new JsonNetResult { Data = data };
                        break;
                    case "jsonp":
                        result = new JsonpNetResult { Data = data };
                        break;
                }
            }

            return result;
        }
    }
}