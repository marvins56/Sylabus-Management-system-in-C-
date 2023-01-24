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

namespace SMIS.Controllers
{
    [Authorize]
    public class SubTopicsController : Controller
    {
        private SMISEntities2 db = new SMISEntities2();

        // GET: SubTopics
        public async Task<ActionResult> Index()
        {
            var subTopicsTables = db.SubTopicsTables.Include(s => s.ClassTable).Include(s => s.TopicsTable).Include(s => s.Term).Include(s => s.Year);
            return View(await subTopicsTables.ToListAsync());
        }

        // GET: SubTopics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubTopicsTable subTopicsTable = await db.SubTopicsTables.FindAsync(id);
            if (subTopicsTable == null)
            {
                return HttpNotFound();
            }
            return View(subTopicsTable);
        }

        // GET: SubTopics/Create
        public ActionResult Create()
        {
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name");
            ViewBag.Topic_id = new SelectList(db.TopicsTables, "Topic_id", "Topic_Name");
            ViewBag.Term_Id = new SelectList(db.Terms, "Term_Id", "term1");
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SubTopics_id,Topic_id,SubTopic,Overview,IsComplete,Datetime,Class_id,year_Id,Term_Id,File")] SubTopicsTable subTopicsTable)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ids = Convert.ToInt32(Session["redirectid"]);
                    if (ids > 0)
                    {
                        if (subTopicsTable.Class_id != (Convert.ToInt32(Session["classid"])))
                        {
                            TempData["error"] = "Kindly select exact class changes are being made";
                        }
                        else
                        {
                            db.SubTopicsTables.Add(subTopicsTable);
                            await db.SaveChangesAsync();
                            TempData["success"] = "Topic ADDED Successfuly";
                            return RedirectToAction("Detail_Dashboard", "Users", new { id = ids });
                        }

                    }
                }
            }catch(Exception e)
            {
                TempData["error"] = e.Message;
            }

            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", subTopicsTable.Class_id);
            ViewBag.Topic_id = new SelectList(db.TopicsTables, "Topic_id", "Topic_Name", subTopicsTable.Topic_id);
            ViewBag.Term_Id = new SelectList(db.Terms, "Term_Id", "term1", subTopicsTable.Term_Id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", subTopicsTable.year_Id);
            return View(subTopicsTable);
        }

        // GET: SubTopics/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubTopicsTable subTopicsTable = await db.SubTopicsTables.FindAsync(id);
            if (subTopicsTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", subTopicsTable.Class_id);
            ViewBag.Topic_id = new SelectList(db.TopicsTables, "Topic_id", "Topic_Name", subTopicsTable.Topic_id);
            ViewBag.Term_Id = new SelectList(db.Terms, "Term_Id", "term1", subTopicsTable.Term_Id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", subTopicsTable.year_Id);
            return View(subTopicsTable);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SubTopics_id,Topic_id,SubTopic,Overview,IsComplete,Datetime,Class_id,year_Id,Term_Id,File")] SubTopicsTable subTopicsTable)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ids = Convert.ToInt32(Session["redirectid"]);
                    if (ids > 0)
                    {
                        db.Entry(subTopicsTable).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        TempData["success"] = "Topic Changes Saved Successfuly";
                        return RedirectToAction("Detail_Dashboard", "Users", new { id = ids });
                    }
                }
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", subTopicsTable.Class_id);
            ViewBag.Topic_id = new SelectList(db.TopicsTables, "Topic_id", "Topic_Name", subTopicsTable.Topic_id);
            ViewBag.Term_Id = new SelectList(db.Terms, "Term_Id", "term1", subTopicsTable.Term_Id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", subTopicsTable.year_Id);
            return View(subTopicsTable);
        }

        // GET: SubTopics/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubTopicsTable subTopicsTable = await db.SubTopicsTables.FindAsync(id);
            if (subTopicsTable == null)
            {
                return HttpNotFound();
            }
            return View(subTopicsTable);
        }

        // POST: SubTopics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                SubTopicsTable subTopicsTable = await db.SubTopicsTables.FindAsync(id);
                db.SubTopicsTables.Remove(subTopicsTable);
                await db.SaveChangesAsync();
                TempData["success"] = "topic DELETED successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index");
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
