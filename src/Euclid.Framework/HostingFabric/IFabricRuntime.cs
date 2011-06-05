namespace Euclid.Framework.HostingFabric
{
	/// <summary>
	/// The direct implementation of the requirements of the runtime fabric for a specific
	/// runtime environment such as Azure, EC2, etc.
	/// </summary>
	public interface IFabricRuntime<TInstanceId>
	{
		TInstanceId CreateInstance();
		void GetInstanceState(TInstanceId id);
		void Shutdown(TInstanceId id);
		void Terminate(TInstanceId id);
		void ScaleUp(TInstanceId id);
		void ScaleDown(TInstanceId id);
	}
}