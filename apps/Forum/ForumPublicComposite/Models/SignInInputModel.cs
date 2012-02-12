namespace ForumComposite.Models
{
	// SELf it would be nice to have a cleaner way of handling models that aren't mapped to commands
	public class SignInInputModel
	{
		public string Password { get; set; }

		public string Username { get; set; }
	}
}