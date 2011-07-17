using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PublicForum.Controllers
{
    public class PostController : Controller
    {
        //
        // GET: /Home/

        public ActionResult List()
        {
            return View();
        }

				public ActionResult Thread()
				{
					return View();
				}

    }
}
