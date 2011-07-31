using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.Metadata
{
	public interface IExtractor
	{
		ICommand CreateCommand(Type commandType);
		IList<Type> GetVisibleCommandTypes();
	}
}