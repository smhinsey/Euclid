using System;
using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class AvailableAvatars : SyntheticReadModel
	{
		public IList<ForumAvatar> Avatars { get; set; }
		public int TotalAvatars { get; set; }
		public string ForumName { get; set; }
		public Guid ForumIdentifier { get; set; }
	}
}