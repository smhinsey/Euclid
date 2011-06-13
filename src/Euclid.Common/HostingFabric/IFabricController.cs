using System;
using System.Collections.Generic;
using Euclid.Common.ServiceHost;

namespace Euclid.Common.HostingFabric
{
	/// <summary>
	/// An IFabricController instance runs inside an IFabricRuntime and is responsible for obtaining 
	/// configured instances of IServiceHosts and installing them into the runtime. It is also responsible
	/// for managing the lifecycle of those service hosts.
	/// </summary>
	public interface IFabricController
	{
		FabricControllerState State { get; }
		void StartServiceHosts();
		void StopServiceHosts();
		void InstallServiceHost(IServiceHost serviceHost);
		IList<Type> ServiceHostTypes { get; }

		// there should probably be status reporting/metrics stuff here, but we'll work that in later in a more
		// systematic way

		// i'm not quite sure yet how we want to expose scalability & management at the controller level.
		// one option i am considering is making it responsive to commands, but this is a well travelled road
		// and it led nowhere in Kilauea v1.
	}
}