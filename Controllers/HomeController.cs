using SMIS.Models;
using SMIS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMIS.Controllers
{
    public class HomeController : Controller
    {
        private SMISEntities db = new SMISEntities();
        public ActionResult Index()
        {
            return View();
        }
        

    }
}