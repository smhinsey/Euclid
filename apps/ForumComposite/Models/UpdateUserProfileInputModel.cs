using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace ForumComposite.Models
{
	public class UpdateUserProfileInputModel : DefaultInputModel
	{
		public UpdateUserProfileInputModel()
		{
			CommandType = typeof(UpdateUserProfile);
		}

		public string AvatarUrl { get; set; }

		public string Email { get; set; }

		public Guid UserIdentifier { get; set; }
	}
}