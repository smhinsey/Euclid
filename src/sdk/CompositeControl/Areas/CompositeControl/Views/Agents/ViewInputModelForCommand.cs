﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using CompositeControl.Areas.CompositeControl.Models;
using Euclid.Framework.Models;

namespace CompositeControl.Areas.CompositeControl.Views.Agents
{
	[GeneratedCode("MvcRazorClassGenerator", "1.0")]
	[PageVirtualPath("~/Areas/CompositeControl/Views/Agents/ViewInputModelForCommand.cshtml")]
	public class _Page_Areas_CompositeControl_Views_Agents_ViewInputModelForCommand_cshtml : WebViewPage<IInputModel>
	{
#line hidden

		protected HttpApplication ApplicationInstance
		{
			get
			{
				return ((Context.ApplicationInstance));
			}
		}

		public override void Execute()
		{
			WriteLiteral("\r\n<h1>");

			Write(ViewBag.Title);

			WriteLiteral("</h1>\r\n\r\n");

			using (Html.BeginForm("Publish", "Agents", FormMethod.Post))
			{
				Write(Html.ValidationSummary(true));

				WriteLiteral("    <fieldset>\r\n        ");

				Write(Html.EditorFor(p => Model));

				WriteLiteral("\r\n    </fieldset>\r\n");

				WriteLiteral("    <input type=\"submit\" value=\"Publish Command\" />\r\n");
			}

			WriteLiteral("\r\n");

			var footer = (FooterLinkModel)ViewBag.Navigation;
			Html.RenderPartial("MetadataLinks", footer);
		}
	}
}