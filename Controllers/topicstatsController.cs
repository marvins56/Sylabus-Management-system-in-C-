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
using System.ComponentModel;

namespace SMIS.Controllers
{
    [Authorize]
    public class topicstatsController : Controller
    {
        private SMISEntities db = new SMISEntities();

        // GET: topicstats
        public async Task<ActionResult> Index()
        {
            return View(await db.topicstats.ToListAsync());
        }
        [ChildActionOnly]
        public PartialViewResult progressbar(int? weekid)
        {
            if (weekid == null)
            {
                TempData["error"] = "ERROR INVALID ID";
            }
            else
            {
                try
                {
                    var subject = Convert.ToInt32(Session["subjectid"]);
                    var classid = Convert.ToInt32(Session["classid"]);
                    var result = db.topicstats.Where(a => a.weekid == weekid && a.Class_id == classid && a.Subject_id == subject).ToList();
                    TempData["info"] = "Fetching Data please Wait..";
                    ViewBag.topicNames = db.TopicsTables.Where(a => a.Class_id == classid && a.Subject_id == subject && a.Week_id == weekid).Select(x => x.Topic_id).ToList();

                    return PartialView("progressbar", result);
                }
                catch (Exception e)
                {
                    TempData["error"] = e.Message;
                }

            }

            

            return PartialView("progressbar",null);

        }
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            topicstat topicstat = await db.topicstats.FindAsync(id);
            if (topicstat == null)
            {
                return HttpNotFound();
            }
            return View(topicstat);
        }

        // GET: topicstats/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Progress_dash(int? weeksid)
        {
            try
            {
                if (weeksid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Session["weekid"] = weeksid;
                var weeksdetails = Get_weeks_info();
                var topicsdetails = Get_Topics_info(weeksid);
                var subtopicsetails = Get_subtopics_info(weeksid);
                var statisticsdata = Get_stats_info(weeksid);

                var Progress_dash = new Progress_dash();

                Progress_dash.Weeks = weeksdetails;
                Progress_dash.Topics = topicsdetails;
                Progress_dash.subtopics = subtopicsetails;
                Progress_dash.topicstats  = statisticsdata;
                return View(Progress_dash);
            }catch(Exception e)
            {
                TempData["error"] = e.Message;
                return View();
            }
        }

        public List<WeeksTable> Get_weeks_info()
        {
            return (db.WeeksTables.ToList());
        }

        public List<topicstat> Get_stats_info(int? weekid)
        {
            try
            {
                var subid = Convert.ToInt32(Session["subjectid"]);
                var classids = Convert.ToInt32(Session["classid"]);

                if (weekid != 0)
                {
                    
                    var data = db.topicstats.Where(a => a.Subject_id == subid && a.Class_id == classids  && a.weekid == weekid).ToList();
                    return (data);
                }
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
                return null;
            }
            return null;
        }
        public List<TopicsTable> Get_Topics_info(int? id1)
        {
            try
            {
                var subid = Convert.ToInt32(Session["subjectid"]);
                var classids = Convert.ToInt32(Session["classid"]);
                
                    var data = db.TopicsTables.Where(a => a.Subject_id == subid && a.Class_id == classids).ToList();
                    return (data);
                
            }catch(Exception e)
            {
                TempData["error"] = e.Message;
                return null;
            }

        }
        public List<SubTopicsTable> Get_subtopics_info(int? weekid)
        {
            try
            {
                var subid = Convert.ToInt32(Session["subjectid"]);
                var classids = Convert.ToInt32(Session["classid"]);

                if (weekid != 0)
                {
                    var topicid = db.TopicsTables.Where(a => a.Subject_id == subid && a.Class_id == classids && a.Week_id == weekid).Select(a => a.Topic_id).FirstOrDefault();
                    var sutopics = db.SubTopicsTables.Where(a => a.Class_id == classids && a.Topic_id == topicid).ToList();

                    return sutopics;
                }
            }catch(Exception e)
            {
                TempData["error"] = e.Message;
                return null;
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,value,weekid,Subject_id,Topic_id,Class_id,Datetime")] topicstat topicstat)
        {
            if (ModelState.IsValid)
            {
                topicstat.Datetime = DateTime.Now;
                db.topicstats.Add(topicstat);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(topicstat);
        }
        // GET: topicstats/Create
        public ActionResult add()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> add([Bind(Include = "Id,value,weekid,Subject_id,Topic_id,Class_id,Datetime")] topicstat topicstat)
        {
            if (ModelState.IsValid)
            {
                var redid = Convert.ToInt32(Session["redirectid"]);
                try
                {
                    topicstat.Datetime = DateTime.Now;
                    db.topicstats.Add(topicstat);

                    await db.SaveChangesAsync();
                    TempData["Info"] = "UPDATE STATUS";
                    TempData["success"] = "Topic progress updated successsfully";

                    return RedirectToAction("Detail_Dashboard", "Users",new { id = redid });
                }catch(Exception e)
                {
                    TempData["error"] = e.Message;
                }
            }

            return View(topicstat);
        }
        // GET: topicstats/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            topicstat topicstat = await db.topicstats.FindAsync(id);
            if (topicstat == null)
            {
                return HttpNotFound();
            }
            return View(topicstat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,value,weekid,Subject_id,Topic_id,Class_id")] topicstat topicstat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topicstat).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(topicstat);
        }

        // GET: topicstats/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            topicstat topicstat = await db.topicstats.FindAsync(id);
            if (topicstat == null)
            {
                return HttpNotFound();
            }
            return View(topicstat);
        }

        // POST: topicstats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            topicstat topicstat = await db.topicstats.FindAsync(id);
            db.topicstats.Remove(topicstat);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
