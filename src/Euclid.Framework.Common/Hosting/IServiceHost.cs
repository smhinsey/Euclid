using System;

namespace Euclid.Framework.Common.Hosting
{
	/// <summary>
	/// IServiceHost implements a particular approach for parallelizing the execution of installed
	/// hosted services.
	/// </summary>
	public interface IServiceHost
	{
		ServiceHostState State { get; }
		HostedServiceState GetState(Guid id);
		Guid Install(IHostedService service);
		void ScaleDown(Guid id);
		void ScaleUp(Guid id);
		void Start(Guid id);
		void Terminate(Guid id);
	}
}