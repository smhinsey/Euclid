using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcForm BeginFormForInputModel(this HtmlHelper helper, IInputModel inputModel)
        {
            // jt: we can deduce the AgentSystemName & PartName w/out requiring them to be explicitly set on the inputmodel
            var form = helper.BeginForm("Publish", "Agents", new { area = "CompositeInspector" }, FormMethod.Post);

            var tagBuilder = new TagBuilder("input");
            tagBuilder.Attributes.Add("type", "hidden");
            tagBuilder.Attributes.Add("name", "agentSystemName");
            tagBuilder.Attributes.Add("value", inputModel.AgentSystemName);
            tagBuilder.Attributes.Add("id", "inputmodel-agentsystemname");
            helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.SelfClosing));

            tagBuilder = new TagBuilder("input");
            tagBuilder.Attributes.Add("type", "hidden");
            tagBuilder.Attributes.Add("name", "partName");
            tagBuilder.Attributes.Add("value", inputModel.PartName);
            tagBuilder.Attributes.Add("id", "inputmodel-partname"); 
            helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.SelfClosing));

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