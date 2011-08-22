using System;

namespace Euclid.Framework.AgentMetadata.PartCollection
{
	internal class PartNotRegisteredException : Exception
	{
		public PartNotRegisteredException(Type typeReceived)
			: base(string.Format("the type {0} is not supported by this agent part collection"))
		{
			this.PartTypeName = typeReceived.FullName;
		}

		public PartNotRegisteredException(string name, string nameSpace)
			: base(string.Format("the type {0}.{1} is not supported by this agent part collection", name, nameSpace))
		{
			this.PartTypeName = string.Format("{0}.{1}", name, nameSpace);
		}

		public string PartTypeName { get; private set; }
	}
}