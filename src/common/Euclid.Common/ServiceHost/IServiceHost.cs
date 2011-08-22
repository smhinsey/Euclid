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
		IDictionary<Guid, IHostedService> Services { get; }

		ServiceHostState State { get; }

		void Cancel(Guid id);

		void CancelAll();

		IList<Exception> GetExceptionsThrownByHostedServices();

		HostedServiceState GetState(Guid id);

		Guid Install(IHostedService service);

		void Start(Guid id);

		void StartAll();
	}
}