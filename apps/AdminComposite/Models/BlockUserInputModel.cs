using System;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.AgentMetadata.Extensions;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class BlockOperationInputModel : DefaultInputModel
	{
		private readonly Type _blockCommand;
		private readonly Type _unblockCommand;

		public BlockOperationInputModel()
		{
			_blockCommand = typeof (BlockUser);
			_unblockCommand = typeof (UnblockUser);
			AgentSystemName = CommandType.Assembly.GetAgentMetadata().SystemName;
		}

		public new Type CommandType
		{
			get { return UserIsBlocked ? _unblockCommand : _blockCommand; }
		}

		public Guid UserIdentifier { get; set; }

		public bool UserIsBlocked { get; set; }

		public string BlockOperationName
		{
			get
			{ return UserIsBlocked ? _unblockCommand.Name : _blockCommand.Name;
			}
		}
	}
}