namespace ForumComposite.ViewModels.Profile
{
	public class ProfileRecentActivityViewModel
	{
		public ForumAgent.ReadModels.ForumUser User { get; set; }

		public bool IsCurrentUser { get; set; }
	}
}