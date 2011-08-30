using System;
using MvcContrib.PortableAreas;

namespace CompositeInspector.Messages
{
	public class ContainerRegistrationMessage : ICommandMessage<ContainerRegistrationResult>
	{
		public Type ImplementationType { get; set; }
		public Type ServiceType { get; set; }
		public ContainerRegistrationResult Result { get; set; }
	}

	public class ContainerRegistrationResult : ICommandResult
	{
		public bool Success { get; set; }
	}
}