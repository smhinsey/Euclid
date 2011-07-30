using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Euclid.Agent;
using Euclid.Composites.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Metadata
{
    // jt: provides access to AgentInfo.cs
    public class AgentInfo : IAgentInfo
    {
        private readonly Assembly _agent;
        private readonly IList<Type> _supportedCommands = new List<Type>();
        private bool _isValid = true;

        public AgentInfo(Assembly agent)
        {
            _agent = agent;
            _isValid = agent.ContainsAgent();

            if (_isValid)
            {
                CommandNamespace = agent.GetCommandNamespace();
                CommandProcessorNamespace = agent.GetProcessorNamespace();
                QueryNamespace = agent.GetQueryNamespace();
                SystemName = agent.GetAgentSystemName();
                FriendlyName = agent.GetAgentName();
            }

            agent
                .GetTypes()
                .Where(t =>
                       t.Namespace == CommandNamespace &&
                       typeof (ICommand).IsAssignableFrom(t))
                .ToList()
                .ForEach(_supportedCommands.Add);
        }


        private void EnsureAgent()
        {
            if (!_isValid)
            {
                throw new AssemblyNotAgentException(_agent);
            }
        }

        public string CommandNamespace { get; private set; }
        public string CommandProcessorNamespace { get; private set; }
        public string FriendlyName { get; private set; }
        public string QueryNamespace { get; private set; }
        public string SystemName { get; private set; }

        public bool SupportsCommand<T>() where T : ICommand
        {
            EnsureAgent();

            return _supportedCommands.Contains(typeof (T));
        }

        public ICommand GetCommand(string commandName)
        {
            EnsureAgent();

            var command = _supportedCommands.Where(c => c.Name == commandName).FirstOrDefault();

            if (command == null)
            {
                throw new UnsupportedCommandException(SystemName, CommandNamespace, commandName);
            }

            return Activator.CreateInstance(command) as ICommand;
        }

        public ICommand GetCommand<T>() where T : ICommand
        {
            if (!SupportsCommand<T>())
            {
                throw new UnsupportedCommandException(SystemName, typeof (T));
            }

            return Activator.CreateInstance(typeof (T)) as ICommand;
        }

        public IEnumerable<Type> GetCommands()
        {
            return _supportedCommands;
        }
    }
}