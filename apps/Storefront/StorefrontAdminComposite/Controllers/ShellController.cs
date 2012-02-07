using System;
using System.Web.Mvc;

namespace StorefrontAdminComposite.Controllers
{
	public class ShellController : AdminController
	{
		public ActionResult Index()
		{
			if(Publisher == null)
			{
				throw new Exception();
			}

			return View();
		}
	}
}