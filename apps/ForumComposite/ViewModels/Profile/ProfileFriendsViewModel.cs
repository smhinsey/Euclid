namespace ForumComposite.ViewModels.Profile
{
	public class ProfileFriendsViewModel
	{
		public ForumAgent.ReadModels.ForumUser User { get; set; }

		public bool IsCurrentUser { get; set; }
	}
}