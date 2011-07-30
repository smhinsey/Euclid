using System;
using System.Collections.Generic;

namespace Euclid.Framework.Cqrs.Metadata
{
    public interface IAgentInfo
    {
        string CommandNamespace { get; }
        string CommandProcessorNamespace { get; }
        string FriendlyName { get; }
        string QueryNamespace { get; }
        string SystemName { get;  }

        bool SupportsCommand<T>() where T : ICommand;
        ICommand GetCommand<T>() where T : ICommand;
        ICommand GetCommand(string commandName);
        IEnumerable<Type> GetCommands();
    }
}