namespace ForumComposite.ViewModels.Profile
{
	public class ProfileBadgesViewModel
	{
		public ForumAgent.ReadModels.ForumUser User { get; set; }

		public bool IsCurrentUser { get; set; }
	}
}