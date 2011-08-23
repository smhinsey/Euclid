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
		/// <summary>
		/// Gets the hosted services configured to execute within the service host instance.
		/// </summary>
		IDictionary<Guid, IHostedService> Services { get; }

		/// <summary>
		/// Gets the current state of the service host.
		/// </summary>
		ServiceHostState State { get; }

		/// <summary>
		/// Cancels the execution of the specified hosted service.
		/// </summary>
		/// <param name="id">the identifier of the hosted service to cancel.</param>
		void Cancel(Guid id);

		/// <summary>
		/// Cancels all hosted services in the service host.
		/// </summary>
		void CancelAll();

		/// <summary>
		/// Obtains the exceptions thrown by the hosted services within the service host.
		/// </summary>
		/// <returns>A list of all exceptions thrown since last call.</returns>
		IList<Exception> GetExceptionsThrownByHostedServices();

		/// <summary>
		/// Retrieves the state of a specific hosted service.
		/// </summary>
		/// <param name="id">The identifier of the hosted service.</param>
		/// <returns>The hosted service's state.</returns>
		HostedServiceState GetState(Guid id);

		/// <summary>
		/// Installs a hosted service into the service host.
		/// </summary>
		/// <param name="service">An instance of the hosted service.</param>
		/// <returns>An identifier for interacting with the installed service.</returns>
		Guid Install(IHostedService service);

		/// <summary>
		/// Starts the specified hosted service.
		/// </summary>
		/// <param name="id">The service to start.</param>
		void Start(Guid id);

		/// <summary>
		/// Starts all installed hosted services.
		/// </summary>
		void StartAll();
	}
}