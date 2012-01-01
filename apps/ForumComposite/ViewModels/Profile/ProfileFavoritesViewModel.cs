using System.Collections.Generic;

namespace ForumComposite.ViewModels.Profile
{
	public class ProfileFavoritesViewModel
	{
		public ForumAgent.ReadModels.ForumUser User { get; set; }

		public bool IsCurrentUser { get; set; }

		public IList<ForumAgent.ReadModels.ForumUserFavorite> Favorites { get; set; }
	}
}