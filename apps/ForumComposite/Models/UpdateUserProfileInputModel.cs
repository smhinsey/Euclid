using System;
using Euclid.Composites.Mvc.Models;

namespace ForumComposite.Models
{
	public class UpdateUserProfileInputModel : InputModelBase
	{
		public string AvatarUrl { get; set; }

		public string Email { get; set; }

		public Guid UserIdentifier { get; set; } 
	}
}