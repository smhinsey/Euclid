using System;

namespace Euclid.Framework.HostingFabric
{
	/// <summary>
	/// 	The direct implementation of the requirements of the runtime fabric for a specific
	/// 	runtime environment such as Azure, EC2, etc.
	/// </summary>
	public interface IServiceHost
	{
		ServiceHostState State { get; }
		HostedServiceState GetState(Guid id);
		Guid Install(IHostedService service);
		void Pause(Guid id);
		void ScaleDown(Guid id);
		void ScaleUp(Guid id);
		void Start(Guid id);
		void Terminate(Guid id);
	}
}