using Euclid.Common.Storage.Model;
using ProfileAgent.ReadModel;

namespace ProfileAgent.Queries
{
	public class UserQueries : IModelRepository<User>
	{
		private readonly ISimpleRepository<User> _repository;

		public UserQueries(ISimpleRepository<User> repository)
		{
			_repository = repository;
		}

		public bool Authenticate(string username, string password)
		{
			// TODO: implement safe hashing/salting and all that noise
			return true;
		}
	}
}