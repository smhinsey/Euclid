using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata.Exceptions;
using Euclid.Framework.Metadata.Extensions;
using Euclid.Framework.Models;

namespace Euclid.Framework.Metadata
{
    internal class CommandMetadataCollection : PartCollectionsBase, ICommandMetadataCollection
    {

        public CommandMetadataCollection(Assembly agent)
        {
            Initialize<ICommand>(agent, agent.GetCommandNamespace());
        }
        
        public ITypeMetadata GetMetadata<TImplementationType>() where TImplementationType : IAgentPart
        {
            return GetMetadata(typeof (TImplementationType));
        }

        public ITypeMetadata GetMetadata(Type agentPartImplementationType)
        {
            var metadata = this.Where(x =>
                                           x.Namespace == agentPartImplementationType.Namespace &&
                                           x.Name == agentPartImplementationType.Name).FirstOrDefault();

            if (metadata == null)
            {
                throw new PartNotRegisteredException(agentPartImplementationType);
            }

            return metadata;
        }

        public ITypeMetadata GetMetadata(string agentPartImplementationName)
        {
            var partImplementationType = this.Where(m => m.Name == agentPartImplementationName).Select(m => m.Type).FirstOrDefault();

            if (partImplementationType == null)
            {
                throw new PartNotRegisteredException(agentPartImplementationName, Namespace);
            }

            return GetMetadata(partImplementationType);
        }

        public bool Registered(string agentPartImplementationName)
        {
            return this.Where(p => p.Name == agentPartImplementationName).Any();
        }

        public bool Registered<TImplementationType>()
        {
            return Registered(typeof (TImplementationType));
        }

        public bool Registered(Type agentPartImplementationType)
        {
            GuardAgentPart(agentPartImplementationType);

            return this.Where(x => 
                            x.Namespace == agentPartImplementationType.Namespace && 
                            x.Name == agentPartImplementationType.Name )
                        .Any();
        }

        /*
        public IInputModel GetInputModel(Type commandType)
        {
            var metadata = this.Where(x => x.Type == commandType).FirstOrDefault();

            if (metadata == null)
            {
                throw new CommandNotRegisteredException(commandType);
            }

            var inputModel = Activator.CreateInstance(typeof(InputModel)) as IInputModel;

            if (inputModel == null)
            {
                throw new CannotCreateInputModelException(typeof(InputModel));
            }

            inputModel.AgentSystemName = metadata.Type.Assembly.GetAgentMetadata().SystemName;
            
            metadata
                .GetAttributes(typeof(InputProperty))
                .ToList()
                .ForEach(inputModel.Properties.Add);

            return inputModel;
        }
        */

        private void GuardAgentPart(Type agentPartImplementationType)
        {
            if (!typeof(ICommand).IsAssignableFrom(agentPartImplementationType))
            {
                throw new InvalidAgentPartImplementation(agentPartImplementationType);
            }
        }
    }
}