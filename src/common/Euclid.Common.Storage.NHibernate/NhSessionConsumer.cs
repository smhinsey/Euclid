using Euclid.Common.Logging;
using Euclid.Common.Storage.Model;
using NHibernate;

namespace Euclid.Common.Storage.NHibernate
{
	public abstract class NhSessionConsumer : ILoggingSource
	{
		private readonly ISession _session;

		protected NhSessionConsumer(ISession session)
		{
			_session = session;
		}

		public ISession GetCurrentSession()
		{
			if (_session.IsOpen)
			{
				return _session;
			}

			this.WriteErrorMessage("The current session is closed or unavailable. Session.IsOpen={0} Session.IsConnected={1}",
			                       null, _session.IsOpen, _session.IsConnected);

			throw new ModelRepositoryException("The current session is closed");
		}
	}
}