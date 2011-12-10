using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Extensions
{
	public static class HtmlHelperExtensions
	{
		public static MvcForm BeginFormForInputModel(
			this HtmlHelper helper,
			IInputModel inputModel,
			bool overrideRedirect = false,
			string alternateRedirectUrl = "",
			string formId = null)
		{
			if (inputModel == null)
			{
				throw new ArgumentNullException("inputModel");
			}

			if (string.IsNullOrEmpty(formId))
			{
				formId = Guid.NewGuid().ToString();
			}

			// jt: we can deduce the AgentSystemName & PartName w/out requiring them to be explicitly set on the inputmodel
			var form = helper.BeginForm("Publish", "Agents", new { area = "CompositeInspector" }, FormMethod.Post, new { id = formId, encType = "multipart/form-data" });

			var tagBuilder = new TagBuilder("input");
			tagBuilder.Attributes.Add("type", "hidden");
			tagBuilder.Attributes.Add("name", "agentSystemName");
			tagBuilder.Attributes.Add("value", inputModel.AgentSystemName);
			tagBuilder.Attributes.Add("id", "inputmodel-agentsystemname");
			helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.SelfClosing));
			helper.ViewContext.Writer.Write(Environment.NewLine);

			tagBuilder = new TagBuilder("input");
			tagBuilder.Attributes.Add("type", "hidden");
			tagBuilder.Attributes.Add("name", "partName");
			tagBuilder.Attributes.Add("value", inputModel.PartName);
			tagBuilder.Attributes.Add("id", "inputmodel-partname");
			helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.SelfClosing));
			helper.ViewContext.Writer.Write(Environment.NewLine);

			if (overrideRedirect)
			{
				tagBuilder = new TagBuilder("input");
				tagBuilder.Attributes.Add("type", "hidden");
				tagBuilder.Attributes.Add("name", "alternateRedirectUrl");
				tagBuilder.Attributes.Add("value", alternateRedirectUrl);
				tagBuilder.Attributes.Add("id", "inputmodel-alternateRedirectUrl");
				helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.SelfClosing));
				helper.ViewContext.Writer.Write(Environment.NewLine);
			}

			return form;
		}
	}

	public class RequiredInputModelFieldIsEmptyException : Exception
	{
		public RequiredInputModelFieldIsEmptyException(string fieldName)
			: base(fieldName)
		{
		}
	}
}