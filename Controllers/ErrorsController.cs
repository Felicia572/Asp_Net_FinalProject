using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asp_Net_FinalProject.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Errors
        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}