using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Agent;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata.Extensions;

namespace Euclid.Framework.Metadata
{
    // jt: exposes data from an Agent's AgentInfo.cs file
    public class AgentMetadataService : IAgentMetadata
    {
        public string CommandNamespace { get; private set; }
        public string CommandProcessorNamespace { get; private set; }
        public string FriendlyName { get; private set; }
        public string QueryNamespace { get; private set; }
        public string SystemName { get; private set; }
        public bool IsValid { get; private set; }
        public IEnumerable<IEuclidMetdata> Commands { get; private set; }

        private Assembly _containingAssembly;

        public AgentMetadataService(Assembly agent)
        {
            _containingAssembly = agent;
            IsValid = agent.ContainsAgent();
            var commands = new List<IEuclidMetdata>();
            
            if (IsValid)
            {
                CommandNamespace = agent.GetCommandNamespace();
                CommandProcessorNamespace = agent.GetProcessorNamespace();
                QueryNamespace = agent.GetQueryNamespace();
                SystemName = agent.GetAgentSystemName();
                FriendlyName = agent.GetAgentName();

                foreach (var type in agent.GetTypes())
                {
                    if (type.Namespace == CommandNamespace && typeof(ICommand).IsAssignableFrom(type))
                    {
                        var md = new EuclidMetadata(type);
                        commands.Add(md);
                    }
                }

                agent.GetTypes()
                    .Where(t =>
                           t.Namespace == CommandNamespace &&
                           typeof (ICommand).IsAssignableFrom(t))
                    .ToList()
                    .ForEach(t => commands.Add(new EuclidMetadata(t)));
            }

            Commands = commands;
        }

        public bool SupportsCommand<T>() where T :ICommand
        {
            EnsureAgent();

            return Commands.Any(x => x.Type == typeof(T));
        }

        public ICommand GetCommand<T>() where T: ICommand
        {
            return GetCommand(typeof (T));
        }

        public ICommand GetCommand(Type t)
        {
            EnsureAgent();

            var cmd = Commands.Where(x => x.Type == t).Select(x => x.Type).FirstOrDefault();

            if (cmd == null)
            {
                throw new UnsupportedCommandException(SystemName, t);
            }

            return Activator.CreateInstance(cmd) as ICommand;
        }

        public IEuclidMetdata GetCommandMetadata(string commandName)
        {
            EnsureAgent();

            string fullCommandName = string.Format("{0}.{1}", CommandNamespace, commandName);

            var cmd = Commands.Where(x => x.Name == commandName).FirstOrDefault();

            if (cmd == null)
            {
                throw new UnsupportedCommandException(SystemName, CommandNamespace, commandName);
            }

            return cmd;
        }

        public IEuclidMetdata GetCommandMetadata<T>()
        {
            return GetCommandMetadata(typeof (T).Name);
        }

        private void EnsureAgent()
        {
            if (!IsValid)
            {
                throw new AssemblyNotAgentException(_containingAssembly);
            }
        }
    }
}