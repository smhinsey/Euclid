using System;

namespace Euclid.Framework.HostingFabric
{
	/// <summary>
	/// The direct implementation of the requirements of the runtime fabric for a specific
	/// runtime environment such as Azure, EC2, etc.
	/// </summary>
	public interface IServiceHost
	{
		Guid Install(IHostedService service);
		void Start(Guid id);
		void Pause(Guid id);
		void Terminate(Guid id);
		void ScaleUp(Guid id);
		void ScaleDown(Guid id);
		ServiceHostState GetState(Guid id);
	}
}