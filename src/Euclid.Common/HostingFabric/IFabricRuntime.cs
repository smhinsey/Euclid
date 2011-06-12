namespace Euclid.Common.HostingFabric
{
	// hosts one or more service hosts in an environment such as a long-running WF workflow in Server AppFabric, a Worker Role in Azure,
	// or a windows console for local machine operation
	public interface IFabricRuntime
	{
		void Start();
		void Shutdown();
		FabricRuntimeState State { get; }
	}
}