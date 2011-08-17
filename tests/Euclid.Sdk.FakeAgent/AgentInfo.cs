
d:\Projects\Euclid\platform>@git.exe %*
﻿using Euclid.Framework.Agent.Attributes;
using Euclid.Framework.Agent.Metadata;
using Euclid.Sdk.FakeAgent.Commands;
using Euclid.Sdk.FakeAgent.Processors;
using Euclid.Sdk.FakeAgent.Queries;
using Euclid.Sdk.FakeAgent.ReadModels;

[assembly: AgentSystemName(Value = "SDKTests.FakeAgent")]
[assembly: AgentName(Value = "Fake Agent")]
[assembly: LocationOfCommands(NamespaceOfType = typeof (FakeCommand))]
[assembly: LocationOfQueries(NamespaceOfType = typeof (FakeQuery))]
[assembly: LocationOfProcessors(NamespaceOfType = typeof (FakeCommandProcessor))]
[assembly: LocationOfReadModels(NamespaceOfType = typeof (FakeReadModel))]
d:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

d:\Projects\Euclid\platform>@rem Restore the original console codepage.

d:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
