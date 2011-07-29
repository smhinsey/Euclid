
D:\Projects\Euclid\platform>@git.exe %*
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.Cqrs.Metadata;
using Euclid.SDK.TestingFakes.Composites.InputModel;

namespace Euclid.Composites.Mvc.Controllers
{
    public class CommandController : Controller
    {
        public CommandController()
        {
        }

        public ViewResult Inspect(ICommandMetadata metadata)
        {
            // JT: find input map for metadata.Type
           
            
            if (metadata == null)
            {
                ViewBag.CommandName = "No command specified";
            }
            else
            {
                ViewBag.CommandName = "Found metadata for: " + metadata.Name;
            }

            return View(new FakeInputModel());
        }

        public ViewResult List(IEnumerable<ICommandMetadata> commands, IAgentMetadata agentMetadata)
        {
            ViewBag.Title = "Commands in agent";
            return View(commands.Select(x=>new CommandMetadataModel { AgentMetadata = agentMetadata, CommandMetadata = x}));
        }
    }
}

D:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

D:\Projects\Euclid\platform>@rem Restore the original console codepage.

D:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
