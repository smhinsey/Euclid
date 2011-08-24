﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using AgentPanel.Areas.Metadata.Models;

namespace AgentPanel.Areas.Metadata.Views.Composite
{
	[GeneratedCode("MvcRazorClassGenerator", "1.0")]
	[PageVirtualPath("~/Areas/Metadata/Views/Composite/Index.cshtml")]
	public class _Page_Areas_Metadata_Views_Composite_Index_cshtml : WebViewPage<CompositeModel>
	{
#line hidden

		protected HttpApplication ApplicationInstance
		{
			get { return (Context.ApplicationInstance); }
		}

		public override void Execute()
		{
			WriteLiteral("           \r\n<h1>");


			Write(ViewBag.Title);

			WriteLiteral("</h1>\r\n\r\n");


			if (Model.Agents.Count() > 0)
			{
				WriteLiteral("    <h2>Agents</h2>\r\n");


				WriteLiteral("    <ul>\r\n");


				foreach (var a in Model.Agents)
				{
					WriteLiteral("        <li><a href=\"");


					Write(Url.Action("ViewAgent", "Agents", new {AgentSystemName = a.SystemName}));

					WriteLiteral("\">");


					Write(a.DescriptiveName);

					WriteLiteral("</a></li>\r\n");
				}

				WriteLiteral("    </ul>\r\n");
			}
			else
			{
				WriteLiteral("    <h2 style=\"color:red\">No agents installed</h2>    \r\n");
			}

			WriteLiteral("\r\n");


			if (Model.InputModels.Count() > 0)
			{
				WriteLiteral("    <h2>Input Models</h2>\r\n");


				WriteLiteral("    <ul>\r\n");


				foreach (var m in Model.InputModels)
				{
					WriteLiteral("        <li>");


					Write(m.Name);

					WriteLiteral("</li>\r\n");
				}

				WriteLiteral("    </ul>\r\n");
			}
			else
			{
				WriteLiteral("    <h2 style=\"color:red\">No input models for composite</h2>\r\n");
			}

			WriteLiteral("\r\n");


			if (Model.ConfigurationErrors.Count() > 0)
			{
				WriteLiteral("    <h2>Errors</h2>\r\n");


				WriteLiteral("    <ul>\r\n");


				foreach (var e in Model.ConfigurationErrors)
				{
					WriteLiteral("        <li>");


					Write(e);

					WriteLiteral("</li>\r\n");
				}

				WriteLiteral("    </ul>\r\n");
			}
			else
			{
				WriteLiteral("    <h2>Configuration</h2>\r\n");


				WriteLiteral("    <ul>\r\n        <li>Output Channel: ");


				Write(Model.Settings.OutputChannel.Value.Name);

				WriteLiteral("</li>\r\n        <li>Publisher: ");


				Write(Model.Settings.Publisher.Value.Name);

				WriteLiteral("</li>\r\n        <li>Blob Storage: ");


				Write(Model.Settings.BlobStorage.Value.Name);

				WriteLiteral("</li>\r\n        <li>Command Dispatcher: ");


				Write(Model.Settings.CommandDispatcher.Value.Name);

				WriteLiteral("</li>\r\n        <li>Command Publication Record Mapper: ");


				Write(Model.Settings.CommandPublicationRecordMapper.Value.Name);

				WriteLiteral("</li>\r\n        <li>Message Serializer: ");


				Write(Model.Settings.MessageSerializer.Value.Name);

				WriteLiteral("</li>\r\n        <li>Publication Registry: ");


				Write(Model.Settings.PublicationRegistry.Value.Name);

				WriteLiteral("</li>\r\n    </ul>\r\n");
			}
		}
	}
}
