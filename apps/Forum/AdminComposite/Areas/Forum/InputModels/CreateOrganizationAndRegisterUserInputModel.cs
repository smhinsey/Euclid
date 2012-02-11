using System.ComponentModel.DataAnnotations;
using Euclid.Composites.Mvc.Models;
using Euclid.Composites.Mvc.Validation;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class CreateOrganizationAndRegisterUserInputModel : DefaultInputModel
	{
		public CreateOrganizationAndRegisterUserInputModel()
		{
			CommandType = typeof(CreateOrganizationAndRegisterUser);
		}

		[Required(ErrorMessage = "Street address cannot be blank")]
		[Display(Name="Street Address")]
		public string Address { get; set; }

		[Display(Name="Address 2")]
		public string Address2 { get; set; }

		[Required(ErrorMessage = "City cannot be blank")]
		[Display(Name = "City")]
		public string City { get; set; }

		[Required(ErrorMessage = "Country cannot be blank")]
		[Display(Name = "Country")]
		public string Country { get; set; }

		[Required(ErrorMessage = "Email address cannot be blank")]
		[Display(Name = "Email Address")]
		[RegularExpression(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$")]
		public string Email { get; set; }

		[Required(ErrorMessage = "First name cannot be blank")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name cannot be blank")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Organization name cannot be blank")]
		[Display(Name = "Organization Name")]
		public string OrganizationName { get; set; }

		[Required(ErrorMessage = "Organization slug cannot be blank")]
		[Display(Name = "Organization Slug")]
		[RegularExpression(@"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?")]
		public string OrganizationSlug { get; set; }

		[Required(ErrorMessage = "Website url cannot be blank")]
		[Display(Name = "Website")]
		public string OrganizationUrl { get; set; }

		[Required(ErrorMessage = "Password cannot be blank")]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Password cannot be blank")]
		[Display(Name = "Password")]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "State cannot be blank")]
		[Display(Name = "State/Provence")]
		public string State { get; set; }

		[Required(ErrorMessage = "Username cannot be blank")]
		[UniqueValue("OrganizationUserQueries", "FindByUsername", ErrorMessage="The username is not unique")]
		[Display(Name = "Username")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Postal code cannot be blank")]
		[Display(Name = "Postal Code")]
		public string Zip { get; set; }
	}
}