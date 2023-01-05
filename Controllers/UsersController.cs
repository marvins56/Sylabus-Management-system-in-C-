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

        public ActionResult Dashboard()
        {
            return View();
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
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            db.AspNetUsers.Remove(aspNetUser);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
            
            TempData["subject"] = subject;
            Session["subjectid"] = subjectid;

            var weeksdetails = Get_weeks_info();
            var topicsdetails = Get_Topics_info(id);

            var detailed_Dashboard = new Detail_Dashboard();

            detailed_Dashboard.Weeks = weeksdetails;
            detailed_Dashboard.Topics = topicsdetails;

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
