using System;
using System.Collections.Generic;
using System.Reflection;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.Metadata
{
    public interface IAgentMetadata
    {
        string CommandNamespace { get; }
        string CommandProcessorNamespace { get; }
        string FriendlyName { get; }
        string QueryNamespace { get; }
        string SystemName { get;  }
        bool IsValid { get; }

        IEuclidMetdata GetCommandMetadata(string commandName);
        IEuclidMetdata GetCommandMetadata<T>();
        IEnumerable<IEuclidMetdata> Commands { get; }
        bool SupportsCommand<T>() where T : ICommand;
        ICommand GetCommand<T>() where T : ICommand;
        ICommand GetCommand(Type t);
    }
}