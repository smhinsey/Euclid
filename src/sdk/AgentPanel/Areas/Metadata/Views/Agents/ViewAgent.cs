﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Metadata/Views/Agents/ViewAgent.cshtml")]
    public class _Page_Areas_Metadata_Views_Agents_ViewAgent_cshtml : System.Web.Mvc.WebViewPage<AgentModel>
    {
#line hidden

        public _Page_Areas_Metadata_Views_Agents_ViewAgent_cshtml()
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

WriteLiteral("</h1>\r\n<ul>\r\n    <li>");


   Write(Model.SystemName);

WriteLiteral("</li>\r\n    <li>");


   Write(Model.DescriptiveName);

WriteLiteral("</li>\r\n    <li>");


   Write(Html.DisplayFor(x => x.Commands, "IAgentPart", @Model.Commands));

WriteLiteral("</li>\r\n    <li>");


   Write(Html.DisplayFor(x => x.Queries, "IAgentPart", @Model.Queries));

WriteLiteral("</li>\r\n    <li>");


   Write(Html.DisplayFor(x => x.ReadModels, "IAgentPart", @Model.ReadModels));

WriteLiteral("</li>\r\n</ul>\r\n\r\n");


   Html.RenderPartial("MetadataLinks", Model); 

        }
    }
}