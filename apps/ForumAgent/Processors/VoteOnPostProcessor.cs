using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class VoteOnPostProcessor : DefaultCommandProcessor<VoteOnPost>
	{
		private readonly ISimpleRepository<Post> _repository;

		public VoteOnPostProcessor(ISimpleRepository<Post> repository)
		{
			this._repository = repository;
		}

		public override void Process(VoteOnPost message)
		{
			var post = this._repository.FindById(message.PostIdentifier);

			if (message.VoteUp)
			{
				post.Score++;
			}
			else
			{
				post.Score--;
			}

			this._repository.Update(post);
		}
	}
}