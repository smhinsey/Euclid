namespace Euclid.Framework.HostingFabric
{
	/// <summary>
	/// An IFabricController instance runs wherever agents are hosted and is responsible for maintaining 
	/// the integrity of the hosting environment. This environment may consist of an in-process fabric,
	/// for use by developers, or fabrics for hosting agents on Azure, EC2, or anywhere AppFabric runs.
	/// </summary>
	public interface IFabricController
	{
		FabricControllerState State { get; }
		void Start();
		void Stop();

		// there should probably be status reporting/metrics stuff here, but we'll work that in later in a more
		// systematic way

		// i'm not quite sure yet how we want to expose scalability & management at the controller level.
		// one option i am considering is making it responsive to commands, but this is a well travelled road
		// and it led nowhere in Kilauea v1.
	}
}