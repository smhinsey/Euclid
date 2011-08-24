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

namespace CompositeControl.Areas.CompositeControl.Views.Agents
{
	[GeneratedCode("MvcRazorClassGenerator", "1.0")]
	[PageVirtualPath("~/Areas/CompositeControl/Views/Agents/ViewPartCollection.cshtml")]
	public class _Page_Areas_CompositeControl_Views_Agents_ViewPartCollection_cshtml : WebViewPage<PartCollectionModel>
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

			WriteLiteral("</h1>\r\n\r\n<ul>\r\n");

			foreach (var part in Model.Parts.Collection)
			{
				WriteLiteral("        <li>\r\n            ");

				Write(part.Namespace);

				WriteLiteral(".");

				Write(part.Name);

				WriteLiteral("\r\n            (\r\n            <a href=\"");

				Write(
					Url.Action(
						@Model.NextActionName, "Agents", new { agentSystemName = Model.Parts.AgentSystemName, partName = part.Name }));

				WriteLiteral("\">html</a> | \r\n            <a href=\"");

				Write(
					Url.Action(
						@Model.NextActionName,
						"Agents",
						new { agentSystemName = Model.Parts.AgentSystemName, partName = part.Name, format = "xml" }));

				WriteLiteral("\">xml</a> |\r\n            <a href=\"");

				Write(
					Url.Action(
						@Model.NextActionName,
						"Agents",
						new { agentSystemName = Model.Parts.AgentSystemName, partName = part.Name, format = "json" }));

				WriteLiteral("\">json</a>)\r\n\t\t\t\r\n        </li>\r\n");
			}

			WriteLiteral("</ul>\r\n\r\n");

			Html.RenderPartial("MetadataLinks", Model);
		}
	}
}