using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace ForumComposite.Models
{
	public class MarkPostAsFavoriteInputModel : DefaultInputModel
	{
		public MarkPostAsFavoriteInputModel()
		{
			CommandType = typeof(MarkPostAsFavorite);
		}

		public Guid ForumIdentifier { get; set; }
		public Guid PostIdentifier { get; set; }
		public Guid UserIdentifier { get; set; }
	}
}