using Euclid.Common.Storage.Model;
using Euclid.Common.Storage.NHibernate;
using ForumAgent.ReadModels;

namespace ForumAgent.Queries
{
	public class UserQueries : IModelRepository<User>
	{
		private readonly NhSimpleRepository<User> _repository;

		public UserQueries(NhSimpleRepository<User> repository)
		{
			_repository = repository;
		}

		public bool Authenticate(string username, string password)
		{
			// TODO: implement safe hashing/salting and all that noise

			var session = _repository.GetCurrentSession();

			var category = session.QueryOver<User>()
				.Where(x => x.PasswordHash == password)
				.Where(x => x.PasswordSalt == password)
				.Where(x => x.Username == username);

			return category != null;
		}
	}
}