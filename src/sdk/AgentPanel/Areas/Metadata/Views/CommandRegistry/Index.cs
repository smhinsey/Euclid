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
    using Euclid.Common.Messaging;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Metadata/Views/CommandRegistry/Index.cshtml")]
    public class _Page_Areas_Metadata_Views_CommandRegistry_Index_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
#line hidden

        public _Page_Areas_Metadata_Views_CommandRegistry_Index_cshtml()
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


WriteLiteral("\t\t\t\t \r\n<h2>Publication Records</h2>\r\n\r\n");


 foreach (IPublicationRecord pub in Model.PublicationRecords)
{

WriteLiteral("\t<div>\r\n\t\tId: <a href=\'");


          Write(Url.Action("Details", "CommandRegistry", new { publicationId = pub.Identifier}));

WriteLiteral("\'>");


                                                                                            Write(pub.Identifier);

WriteLiteral("</a>\r\n\t\t<br />\r\n\t\tCreated: ");


      Write(pub.Created);

WriteLiteral("\r\n\t\t<br />\r\n\t\tDispatched: ");


         Write(pub.Dispatched);

WriteLiteral("\r\n\t\t<br />\r\n\t\tError: ");


    Write(pub.Error);

WriteLiteral("\r\n\t\t<br />\r\n\t\tMessage: ");


      Write(pub.ErrorMessage);

WriteLiteral("\r\n\t\t<br />\r\n\t\tType: ");


   Write(pub.MessageType);

WriteLiteral("\r\n\t\t<br />\r\n\t\tLocation: ");


       Write(pub.MessageLocation);

WriteLiteral("\r\n\t</div>\r\n");


    

WriteLiteral("\t<br />\r\n");


}

        }
    }
}
