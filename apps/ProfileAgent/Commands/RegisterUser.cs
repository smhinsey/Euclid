namespace PublicForum.ProfileAgent.Commands
{
	public class RegisterUser
	{
		public string PasswordHash { get; set; }
		public string PasswordSalt { get; set; }
		public string Username { get; set; } 
	}
}