using System;

namespace Euclid.Common.HostingFabric
{
	public class RuntimeFabricConfigurationException : Exception
	{
		public RuntimeFabricConfigurationException(string message, Exception exception) : base(message, exception)
		{
			
		}
	}
}