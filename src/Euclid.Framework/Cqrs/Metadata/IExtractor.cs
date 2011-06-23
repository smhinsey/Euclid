using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euclid.Framework.Cqrs.Metadata
{
    public interface IExtractor
    {
        IList<Type> GetVisibleCommandTypes();
        ICommand CreateCommand(Type commandType);
    }
}
