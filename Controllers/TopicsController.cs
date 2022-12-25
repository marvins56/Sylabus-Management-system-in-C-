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
    public class TopicsController : Controller
    {
        private SMISEntities db = new SMISEntities();

     
public async Task<ActionResult> Index()
        {
            var topicsTables = db.TopicsTables.Include(t => t.ClassTable).Include(t => t.SubjectTable).Include(t => t.Term).Include(t => t.Year).Include(t => t.WeeksTable);
            return View(await topicsTables.ToListAsync());
        }
        [ChildActionOnly]
        public PartialViewResult subjecttopics(int? weekid)
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

                    var result = db.TopicsTables.Where(a => a.Week_id == weekid && a.Class_id == classid && a.Subject_id == subject).ToList();
                    TempData["info"] = "Fetching Data please Wait..";
                    return PartialView("subjecttopics", result);
                }
                catch (Exception e)
                {
                    TempData["error"] = e.Message;
                }

            }

            return PartialView("subjecttopics");

        }

        // GET: Topics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicsTable topicsTable = await db.TopicsTables.FindAsync(id);
            if (topicsTable == null)
            {
                return HttpNotFound();
            }
            return View(topicsTable);
        }

        // GET: Topics/Create
        public ActionResult Create()
        {
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name");
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name");
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1");
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1");
            ViewBag.Week_id = new SelectList(db.WeeksTables, "Week_id", "Week_Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Topic_id,Topic_Name,Class_id,IsComplete,DateTime,Subject_id,Term_id,year_Id,Week_id,Overview,File")] TopicsTable topicsTable)
        {
            if (ModelState.IsValid)
            {
                db.TopicsTables.Add(topicsTable);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", topicsTable.Class_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", topicsTable.Subject_id);
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", topicsTable.Term_id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", topicsTable.year_Id);
            ViewBag.Week_id = new SelectList(db.WeeksTables, "Week_id", "Week_Name", topicsTable.Week_id);
            return View(topicsTable);
        }

        // GET: Topics/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicsTable topicsTable = await db.TopicsTables.FindAsync(id);
            if (topicsTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", topicsTable.Class_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", topicsTable.Subject_id);
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", topicsTable.Term_id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", topicsTable.year_Id);
            ViewBag.Week_id = new SelectList(db.WeeksTables, "Week_id", "Week_Name", topicsTable.Week_id);
            return View(topicsTable);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Topic_id,Topic_Name,Class_id,IsComplete,DateTime,Subject_id,Term_id,year_Id,Week_id,Overview,File")] TopicsTable topicsTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topicsTable).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", topicsTable.Class_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", topicsTable.Subject_id);
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", topicsTable.Term_id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", topicsTable.year_Id);
            ViewBag.Week_id = new SelectList(db.WeeksTables, "Week_id", "Week_Name", topicsTable.Week_id);
            return View(topicsTable);
        }

        // GET: Topics/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicsTable topicsTable = await db.TopicsTables.FindAsync(id);
            if (topicsTable == null)
            {
                return HttpNotFound();
            }
            return View(topicsTable);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TopicsTable topicsTable = await db.TopicsTables.FindAsync(id);
            db.TopicsTables.Remove(topicsTable);
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
