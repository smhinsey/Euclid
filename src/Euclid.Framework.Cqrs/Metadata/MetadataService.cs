using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Euclid.Framework.Cqrs.Metadata
{
    public class MetadataService : IExtractor
    {
        private readonly IList<Type> _commandTypes;
        public MetadataService(Assembly currentAssembly)
        {
            _commandTypes = currentAssembly.GetTypes().Where(type => type.GetInterface(typeof(ICommand).FullName) != null).ToList();
        }

        public IList<Type> GetVisibleCommandTypes()
        {
            return _commandTypes;
        }

        public ICommand CreateCommand(Type commandType)
        {
            return Activator.CreateInstance(commandType) as ICommand;
        }
    }
}
