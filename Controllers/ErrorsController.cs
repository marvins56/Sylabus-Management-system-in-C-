using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMIS.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Errors
        public ActionResult badrequest()
        {
            return View();
        }
        public ActionResult internalError()
        {
            return View();
        }
        public ActionResult notfound()
        {
            return View();
        }

    }
}
