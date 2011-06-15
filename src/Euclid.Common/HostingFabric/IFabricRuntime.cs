namespace Euclid.Common.HostingFabric
{
	/// <summary>
	/// 	An instance of IFabricRuntime encapsulates all that is necessary to host a fully-functional set of IHostedService
	/// 	implementations within a configurable implementation of an IServiceHost. 
	/// 
	/// 	Example implementations include, but are not limited to:
	/// 
	/// 	1) LocalMachine, uses threading to emulate a production environment for a developer
	/// 	2) TopShelf, uses the OSS TopShelf library, which offers support for Windows Services and Console apps
	/// 	3) Azure Worker Role, the Azure equivalent of a Windows Service
	/// 	4) Windows Server AppFabric, using a long-running WF host, run anywhere Windows Server AppFabric runs
	/// </summary>
	public interface IFabricRuntime
	{
		void Configure(IFabricRuntimeSettings settings);
		IFabricRuntimeStatistics GetStatistics();
		void Shutdown();
		void Start();
	}
}