
d:\Projects\Euclid\platform>@git.exe %*
﻿using Euclid.Framework.Agent;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composite.MvcApplication.Models
{
    public class AgentPartModel : FooterLinkModel
    {
        public string AgentSystemName { get; set; }
        public string NextAction { get; set; }
        public IPartCollection Part { get; set; }
    }
}
d:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

d:\Projects\Euclid\platform>@rem Restore the original console codepage.

d:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
