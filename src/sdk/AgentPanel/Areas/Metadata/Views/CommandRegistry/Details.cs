﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AgentPanel.Areas.Metadata.Views.CommandRegistry
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Metadata/Views/CommandRegistry/Details.cshtml")]
    public class _Page_Areas_Metadata_Views_CommandRegistry_Details_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
#line hidden

        public _Page_Areas_Metadata_Views_CommandRegistry_Details_cshtml()
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

WriteLiteral("<div>\r\n\tId: ");


Write(Model.Identifier);

WriteLiteral("\r\n\t<br />\r\n\tCreated: ");


     Write(Model.Created);

WriteLiteral("\r\n\t<br />\r\n\tDispatched: ");


        Write(Model.Dispatched);

WriteLiteral("\r\n\t<br />\r\n\tError: ");


   Write(Model.Error);

WriteLiteral("\r\n\t<br />\r\n\tMessage: ");


     Write(Model.ErrorMessage);

WriteLiteral("\r\n\t<br />\r\n\tType: ");


  Write(Model.MessageType);

WriteLiteral("\r\n\t<br />\r\n\tLocation: ");


      Write(Model.MessageLocation);

WriteLiteral("\r\n</div>\r\n\r\n");


        }
    }
}
