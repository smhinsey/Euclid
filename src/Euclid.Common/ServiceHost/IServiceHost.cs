using System;
using System.Collections.Generic;

namespace Euclid.Common.ServiceHost
{
	/// <summary>
	/// 	IServiceHost implements a particular approach for parallelizing the execution of installed
	/// 	hosted services.
	/// </summary>
	public interface IServiceHost
	{
		int Scale { get; }
		IDictionary<Guid, IHostedService> Services { get; }
		ServiceHostState State { get; }
		HostedServiceState GetState(Guid id);
		Guid Install(IHostedService service);
		void ScaleAllDown();
		void ScaleAllUp();
		void ScaleDown(Guid id);
		void ScaleUp(Guid id);
		void Start(Guid id);
		void StartAll();
		void Stop(Guid id);
		void StopAll();
	}
}