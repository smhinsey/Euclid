
d:\Projects\Euclid\platform>@git.exe %*
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using Euclid.Framework.Agent.Extensions;
//using Euclid.Framework.Agent.Metadata;
//using Euclid.Framework.Agent.Parts;

//namespace Euclid.Framework.Agent
//{
//    public abstract class PartCollectionsBase<TAgentPart> 
//        : IPartCollection where TAgentPart 
//        : IAgentPart
//    {
//        private List<ITypeMetadata> _internal;
//        public Type CollectionType { get; private set; }

//        public ITypeMetadata this[int index]
//        {
//            get { return _internal[index]; }
//            set { _internal[index] = value; }
//        }

//        public string AgentSystemName { get; private set; }

//        public int Count
//        {
//            get { return _internal.Count; }
//        }

//        public bool IsReadOnly
//        {
//            get { return false; }
//        }

//        public string Namespace { get; private set; }

//        public void Add(ITypeMetadata item)
//        {
//            if (Contains(item))
//            {
//                throw new DuplicatePartNameException(CollectionType.Name, item.Name);
//            }

//            _internal.Add(item);
//        }

//        public void Clear()
//        {
//            throw new NotImplementedException();
//        }

//        public bool Contains(ITypeMetadata item)
//        {
//            return _internal.Where(x => x.Name == item.Name).Any();
//        }

//        public void CopyTo(ITypeMetadata[] array, int arrayIndex)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerator<ITypeMetadata> GetEnumerator()
//        {
//            return _internal.GetEnumerator();
//        }

//        //public ITypeMetadata GetMetadata<TImplementationType>() where TImplementationType : IAgentPart
//        //{
//        //    return GetMetadata(typeof (TImplementationType));
//        //}

//        //public ITypeMetadata GetMetadata(Type agentPartImplementationType)
//        //{
//        //    var metadata = this.Where(x =>
//        //                              x.Namespace == agentPartImplementationType.Namespace &&
//        //                              x.Name == agentPartImplementationType.Name).FirstOrDefault();

//        //    if (metadata == null)
//        //    {
//        //        throw new PartNotRegisteredException(agentPartImplementationType);
//        //    }

//        //    return metadata;
//        //}

//        //public ITypeMetadata GetMetadata(string agentPartImplementationName)
//        //{
//        //    var partImplementationType = this.Where(m => m.Name == agentPartImplementationName).Select(m => m.Type).FirstOrDefault();

//        //    if (partImplementationType == null)
//        //    {
//        //        throw new PartNotRegisteredException(agentPartImplementationName, Namespace);
//        //    }

//        //    return GetMetadata(partImplementationType);
//        //}

//        public int IndexOf(ITypeMetadata item)
//        {
//            return _internal.IndexOf(item);
//        }

//        public void Insert(int index, ITypeMetadata item)
//        {
//            _internal.Insert(index, item);
//        }

//        //public bool Registered(string agentPartImplementationName)
//        //{
//        //    return this.Where(p => p.Name == agentPartImplementationName).Any();
//        //}

//        //public bool Registered<TImplementationType>()
//        //{
//        //    return Registered(typeof (TImplementationType));
//        //}

//        //public bool Registered(Type agentPartImplementationType)
//        //{
//        //    guardAgentPart(agentPartImplementationType);

//        //    return this.Where(x =>
//        //                      x.Namespace == agentPartImplementationType.Namespace &&
//        //                      x.Name == agentPartImplementationType.Name)
//        //        .Any();
//        //}

//        public bool Remove(ITypeMetadata item)
//        {
//            throw new NotImplementedException();
//        }

//        public void RemoveAt(int index)
//        {
//            throw new NotImplementedException();
//        }

//        protected void Initialize(Assembly agent, string partNamespace)
//        {
//            _internal = agent.GetTypes()
//                                .Where(type =>
//                                       type.Namespace == partNamespace &&
//                                       typeof (TAgentPart).IsAssignableFrom(type))
//                                .Select(type => new TypeMetadata(type))
//                                .Cast<ITypeMetadata>()
//                                .ToList();

//            CollectionType = typeof (TAgentPart);
//            AgentSystemName = agent.GetAgentSystemName();
//            Namespace = partNamespace;
//        }

//        //private void guardAgentPart(Type agentPartImplementationType)
//        //{
//        //    if (agentPartImplementationType == null)
//        //    {
//        //        throw new ArgumentNullException("agentPartImplementationType");
//        //    }

//        //    if (!typeof (TAgentPart).IsAssignableFrom(agentPartImplementationType))
//        //    {
//        //        throw new InvalidAgentPartImplementationException(agentPartImplementationType);
//        //    }
//        //}

//        public IMetadataFormatter GetFormatter()
//        {
//            var metadata = new TypeMetadata(CollectionType);

//            return FormattableMetadataFactory.GetFormatter(metadata);
//        }
//    }
//}
d:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

d:\Projects\Euclid\platform>@rem Restore the original console codepage.

d:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
