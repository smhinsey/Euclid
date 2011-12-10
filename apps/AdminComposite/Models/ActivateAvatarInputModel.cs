using System;
using Euclid.Composites.Mvc.Models;

namespace AdminComposite.Models
{
	public class ActivateAvatarInputModel : DefaultInputModel
	{
		public Guid AvatarIdentifier { get; set; }
		public bool Active { get; set; }
	}
}