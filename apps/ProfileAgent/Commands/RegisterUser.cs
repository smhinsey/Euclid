using Euclid.Framework.Cqrs;

namespace ProfileAgent.Commands
{
	public class RegisterUser : DefaultCommand
	{
		public string PasswordHash { get; set; }
		public string PasswordSalt { get; set; }
		public string Username { get; set; } 
	}
}