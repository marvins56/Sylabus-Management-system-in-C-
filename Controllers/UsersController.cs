using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMIS.Models;
using SMIS.Models.ViewModel;
using Newtonsoft.Json;

namespace SMIS.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private SMISEntities db = new SMISEntities();

        public async Task<ActionResult> Index()
        {
            return View(await db.AspNetUsers.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["success"] = "Changes made successfully";
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
                db.AspNetUsers.Remove(aspNetUser);
                await db.SaveChangesAsync();
                TempData["success"] = "Changes made successfully";

                return RedirectToAction("Index");
            }catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [ChildActionOnly]
        public PartialViewResult Subject_stats()
        {
            var subid = Convert.ToInt32(Session["subjectid"]);
            var classids = Convert.ToInt32(Session["classid"]);
            var student_failed = db.MidtermMarksTables.Where(a => a.Class_id == classids && a.Subject_id == subid && (a.Marks > 0 && a.Marks <49)).Count();
            var student_average = db.MidtermMarksTables.Where(a => a.Class_id == classids && a.Subject_id == subid && (a.Marks >= 50 && a.Marks <= 75)).Count();
            var student_passed = db.MidtermMarksTables.Where(a => a.Class_id == classids && a.Subject_id == subid && (a.Marks >= 76 && a.Marks <= 100)).Count();

            List<DataPoint2> dataPoints2 = new List<DataPoint2>();
            dataPoints2.Add(new DataPoint2("Below 49 %", student_failed));
            dataPoints2.Add(new DataPoint2("average 50-75 %", student_average));
            dataPoints2.Add(new DataPoint2("average 76-100 %", student_passed));
            ViewBag.class_datapoints = JsonConvert.SerializeObject(dataPoints2);

            return PartialView("Subject_stats");
        }
        public ActionResult Detail_Dashboard(int? id)
        {
            Session["redirectid"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var classid = db.subject_class.Where(a=>a.ID == id).Select(a=>a.Class_id).FirstOrDefault();
            var subjectid = db.subject_class.Where(a => a.ID == id).Select(a => a.Subject_id).FirstOrDefault();
            var subject = db.SubjectTables.Where(a => a.Subject_id == subjectid).Select(a => a.Name).FirstOrDefault();
            Session["cid"] = classid;
            Session["sbid"] = subjectid;

            TempData["subject"] = subject;
            Session["subjectid"] = subjectid;

            var weeksdetails = Get_weeks_info();
            var topicsdetails = Get_Topics_info(id);
            var topicsperclass = get_topics_perclass(classid, subjectid);
            var statperclass =get_stats_perclass(classid, subjectid);
            var videosperclass = get_videos_perclass(classid, subjectid);
          
            var detailed_Dashboard = new Detail_Dashboard();

            detailed_Dashboard.Weeks = weeksdetails;
            detailed_Dashboard.Topics = topicsdetails;
            detailed_Dashboard.topicscount = topicsperclass;
            detailed_Dashboard.videos = videosperclass;
            detailed_Dashboard.stats = statperclass;

            return View(detailed_Dashboard);
        }

        public List<WeeksTable> Get_weeks_info()
        {
            return (db.WeeksTables.ToList());
        }
       
        public List<TopicsTable> Get_Topics_info(int? id1)
        {
            var subid = Convert.ToInt32(Session["subjectid"]);
            var classids = Convert.ToInt32(Session["classid"]);
            if (Convert.ToInt32(Session["subjectid"]) == id1)
            {
                var data = db.TopicsTables.Where(a => a.Subject_id == subid && a.Class_id == classids).ToList();
                return data;
            }
            return null;
        }
        public List<TopicsTable>get_topics_perclass(int? classid,int? subjectid) 
        {
            var res = db.TopicsTables.Where(a => a.Class_id == classid && a.Subject_id == subjectid).ToList();
            return res;
        }
        public List<TopicsTable> get_videos_perclass(int? classid, int? subjectid)
        {
            var res = db.TopicsTables.Where(a => a.Class_id == classid && a.Subject_id == subjectid && a.ContentType =="video/mp4").ToList();
            return res;
        }

        public List<topicstat> get_stats_perclass(int? classid, int? subjectid)
        {
            var res = db.topicstats.Where(a => a.Class_id == classid && a.Subject_id == subjectid ).ToList();
           
            return res;
        }
        
        //public JsonResult Getstats( )
        //{
        //    var classid = Session["cid"];
        //    var subjectid = Session["sbid"];
        //    var res = db.topicstats.Where(a => a.Class_id == classid && a.Subject_id == subjectid).Select(a=>a.value).ToList();
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult Dashboard()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult chart()
        {
            try
            {
                var classid = Convert.ToInt32(Session["cid"]);
                var subjectid = Convert.ToInt32(Session["sbid"]);
                var dta = db.topicstats.Where(a => a.Class_id == classid && a.Subject_id == subjectid).Select(a => new { a.Topic_id, a.value, }).ToList();
                List<DataPoint> dataPoints = new List<DataPoint>();
                foreach(var d in dta)
                {
                    var topicname = db.TopicsTables.Where(a => a.Topic_id == d.Topic_id).Select(a => a.Topic_Name).FirstOrDefault();
                    dataPoints.Add(new DataPoint(topicname, d.value));
                    ViewBag.datapoints = JsonConvert.SerializeObject(dataPoints);
                }

                return PartialView("chart");
            }catch(Exception e)
            {
                TempData["error"] = e.Message;
                return PartialView("chart", null);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
