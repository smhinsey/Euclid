﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AgentPanel.Areas.Metadata.Views.Agents
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Caching;
    using System.Web.DynamicData;
    using System.Web.SessionState;
    using System.Web.Profile;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    using System.Xml.Linq;
    using AgentPanel.Areas.Metadata.Models;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Metadata/Views/Agents/ViewPart.cshtml")]
    public class _Page_Areas_Metadata_Views_Agents_ViewPart_cshtml : System.Web.Mvc.WebViewPage<PartModel>
    {
#line hidden

        public _Page_Areas_Metadata_Views_Agents_ViewPart_cshtml()
        {
        }
        protected System.Web.HttpApplication ApplicationInstance
        {
            get
            {
                return ((System.Web.HttpApplication)(Context.ApplicationInstance));
            }
        }
        public override void Execute()
        {



WriteLiteral("\r\n<h1>");


Write(ViewBag.Title);

WriteLiteral("</h1>\r\n\r\n");


 if (Model.TypeMetadata.Properties.Count() > 0)
{

WriteLiteral("\t<h3>Properties</h3>\r\n");



WriteLiteral("\t<ul>\r\n");


 		foreach (var p in Model.TypeMetadata.Properties)
		{

WriteLiteral("\t\t\t<li>");


  Write(p.Name);

WriteLiteral(" (");


           Write(p.PropertyType.Name);

WriteLiteral(")</li>\r\n");


		}

WriteLiteral("\t</ul>\r\n");


}

WriteLiteral("\r\n");


 if (Model.TypeMetadata.Methods.Count() > 0)
{

WriteLiteral("\t<h3>Methods</h3>\r\n");



WriteLiteral("\t<ul>\r\n");


 		foreach (var m in Model.TypeMetadata.Methods)
		{
			var sb = new StringBuilder();
			foreach (var a in m.Arguments)
			{
				if (sb.Length > 0)
				{
					sb.Append(", ");
				}

				sb.AppendFormat("{0} {1}", a.PropertyType.Name, a.Name);

				if (a.DefaultValue != null)
				{
					sb.AppendFormat("<i>={0}</i>", a.DefaultValue);
				}
			}


WriteLiteral("\t\t\t<li>\r\n\t\t\t\t");


Write(m.ReturnType);

WriteLiteral("\r\n\t\t\t\t");


Write(m.Name);

WriteLiteral(" (");


        Write(Html.Raw(sb.ToString()));

WriteLiteral(")\r\n\t\t\t</li>\r\n");


		}

WriteLiteral("\t</ul>\r\n");


}

WriteLiteral("\r\n");


   Html.RenderPartial("MetadataLinks", Model); 


        }
    }
}
