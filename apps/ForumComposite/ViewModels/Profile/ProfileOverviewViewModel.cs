namespace ForumComposite.ViewModels.Profile
{
	public class ProfileOverviewViewModel
	{
		public ForumAgent.ReadModels.ForumUser User { get; set; }

		public bool IsCurrentUser { get; set; }
	}
}