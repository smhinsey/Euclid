
d:\Projects\Euclid\platform>@git.exe %*
﻿namespace Euclid.Composite.MvcApplication.Models
{
    public class FooterLinkModel
    {
        public string AgentSytemName { get; set; }
        public string PartType { get; set; }
        public string PartDescriptiveName { get; set; }
    }
}
d:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

d:\Projects\Euclid\platform>@rem Restore the original console codepage.

d:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
