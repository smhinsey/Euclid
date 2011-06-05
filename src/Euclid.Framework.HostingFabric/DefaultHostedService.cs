namespace Euclid.Framework.HostingFabric
{
	public abstract class DefaultHostedService : IHostedService
	{
		protected DefaultHostedService(string name)
		{
			Name = name;
		}

		public string Name { get; private set; }
		public HostedServiceState State { get; protected set; }

		public abstract void Start();
		public abstract void Stop();
		public abstract void Pause();
	}
}