﻿using Euclid.Framework.Metadata;

namespace Euclid.Composite.MvcApplication.Models
{
    public class CommandMetadataModel
    {
        public string AgentSystemName { get; set; }
        public ICommandMetadataCollection Commands { get; set; }
    }
}