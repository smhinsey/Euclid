using System;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class BlockUserProcessor : BlockOperation<BlockUser>
	{
		protected override bool Value
		{
			get { return true; }
		}

		protected override Guid GetUserIdentifier(BlockUser command)
		{
			return command.UserIdentifier;
		}
	}

	public class UnblockUserProcessor : BlockOperation<UnblockUser>
	{
		protected override bool Value
		{
			get { return false; }
		}

		protected override Guid GetUserIdentifier(UnblockUser command)
		{
			return command.UserIdentifier;
		}
	}

	public abstract class BlockOperation<T> : DefaultCommandProcessor<T> where T:ICommand
	{
		public ISimpleRepository<ForumUser> UserRepository { get; set; }

		protected abstract bool Value { get; }

		protected abstract Guid GetUserIdentifier(T command);

		public override void Process(T message)
		{
			var userIdentifier = GetUserIdentifier(message);

			var user = UserRepository.FindById(userIdentifier);

			if (user == null)
			{
				throw new UserNotFoundException(string.Format("Could not block the user with Id {0}", userIdentifier));
			}

			user.IsBlocked = Value;
			UserRepository.Save(user);
		}
	}
}