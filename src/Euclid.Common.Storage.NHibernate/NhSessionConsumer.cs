using Euclid.Common.Storage.Model;
using NHibernate;

namespace Euclid.Common.Storage.NHibernate
{
	public abstract class NhSessionConsumer
	{
		private readonly ISession _session;

		protected NhSessionConsumer(ISession session)
		{
			_session = session;
		}

		protected ISession GetCurrentSession()
		{
			if (_session.IsOpen)
			{
				return _session;
			}

			throw new ModelRepositoryException("The current session is closed");
		}
	}
}