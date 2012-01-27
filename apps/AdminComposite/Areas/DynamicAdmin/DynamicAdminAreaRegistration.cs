using System.Web.Mvc;

namespace AdminComposite.Areas.DynamicAdmin
{
	public class DynamicAdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "DynamicAdmin";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
					"DynamicAdmin_default",
					"DynamicAdmin/{controller}/{action}/{id}",
					new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
