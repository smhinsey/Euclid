namespace Euclid.Framework.HostingFabric
{
	/// <summary>
	/// The direct implementation of the requirements of the runtime fabric for a specific
	/// runtime environment such as Azure, EC2, etc.
	/// </summary>
	public interface IServiceHost
	{
		string Install();
		void Start(string id);
		void Pause(string id);
		void Terminate(string id);
		void ScaleUp(string id);
		void ScaleDown(string id);
		ServiceHostState GetState(string id);
	}
}