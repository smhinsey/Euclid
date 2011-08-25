using Euclid.Composites.Mvc.Models;

namespace ForumComposite.Models
{
	public class RegisterUserInputModel : InputModelBase
	{
		public string Password { get; set; }
		
		public string ConfirmationPassword { get; set; }

		public string Username { get; set; }
	}
}