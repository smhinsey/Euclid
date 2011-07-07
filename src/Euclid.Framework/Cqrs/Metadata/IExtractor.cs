using System;
using System.Collections.Generic;

namespace Euclid.Framework.Cqrs.Metadata
{
	public interface IExtractor
	{
		ICommand CreateCommand(Type commandType);
		IList<Type> GetVisibleCommandTypes();
	}
}