namespace ForumComposite.ViewModels.Profile
{
	public class ProfileFavoritesViewModel
	{
		public ForumAgent.ReadModels.ForumUser User { get; set; }

		public bool IsCurrentUser { get; set; }
	}
}