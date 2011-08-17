using System.Reflection;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.Agent.PartCollection
{
	public class CommandPartCollection : PartCollectionBase<ICommand>
	{
		public CommandPartCollection(Assembly agent, string commandNamespace)
			: base(agent, commandNamespace)
		{
		}

		public override string DescriptiveName
		{
			get { return "Commands"; }
		}
	}
}