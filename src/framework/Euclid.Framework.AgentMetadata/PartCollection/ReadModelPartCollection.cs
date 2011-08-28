using System.Reflection;
using Euclid.Framework.Models;

namespace Euclid.Framework.AgentMetadata.PartCollection
{
	public class ReadModelPartCollection : PartCollectionBase<IReadModel>
	{
		public ReadModelPartCollection(Assembly agent, string readModelNamesapce)
			: base(agent, readModelNamesapce)
		{
		}

		public override string DescriptiveName
		{
			get
			{
				return "ReadModels";
			}
		}
	}
}