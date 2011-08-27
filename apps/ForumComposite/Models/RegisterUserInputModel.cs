using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace ForumComposite.Models
{
	public class RegisterUserInputModel : InputModelBase
	{
		public RegisterUserInputModel()
		{
			AgentSystemName = "NewCo.ForumAgent";
			CommandType = typeof(RegisterUser);
		}

		public string ConfirmationPassword { get; set; }

		public string Password { get; set; }

		public string Username { get; set; }
	}
}