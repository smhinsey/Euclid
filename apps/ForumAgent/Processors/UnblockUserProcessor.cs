using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class UnblockUserProcessor : DefaultCommandProcessor<UnblockUser>
	{
		private readonly ISimpleRepository<ForumUser> _userRepository;

		public UnblockUserProcessor(ISimpleRepository<ForumUser> userRepository)
		{
			_userRepository = userRepository;
		}

		public override void Process(UnblockUser message)
		{
			var user = _userRepository.FindById(message.UserIdentifier);
			user.IsBlocked = false;
			user.Modified = DateTime.Now;
			_userRepository.Update(user);
		}
	}
}