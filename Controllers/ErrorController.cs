using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMIS.Controllers
{
    public class ErrorController : Controller
    {
// 400 Status code is call when required page not found type of error

//200 Status code is call when response is ok type of error.


//Status Code attribute
//301 – This code is use to Moved Permanently

//403 – This code is use for Forbidden

//404 – This code is use for Not Found
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult NotFoundException()
        {
            return View();
        }
        public ActionResult badrequest()
        {
            return View();
        }
        public ActionResult internalerror()
        {
            return View();
        }
    }
}