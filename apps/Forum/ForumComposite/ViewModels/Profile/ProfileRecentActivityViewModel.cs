using System.Collections.Generic;

namespace ForumComposite.ViewModels.Profile
{
	public class ProfileRecentActivityViewModel
	{
		public ForumAgent.ReadModels.ForumUser User { get; set; }

		public bool IsCurrentUser { get; set; }

		public IList<ForumAgent.ReadModels.ForumUserAction> Activity { get; set; }
	}
}