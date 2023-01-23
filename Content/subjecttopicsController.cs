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

namespace SMIS.Content
{
    public class subjecttopicsController : Controller
    {
        private SMISEntities2 db = new SMISEntities2();

        // GET: subjecttopics
        public async Task<ActionResult> Index()
        {
            var subjecttopics = db.subjecttopics.Include(s => s.Term).Include(s => s.TopicsTable).Include(s => s.WeeksTable);
            return View(await subjecttopics.ToListAsync());
        }

        // GET: subjecttopics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subjecttopic subjecttopic = await db.subjecttopics.FindAsync(id);
            if (subjecttopic == null)
            {
                return HttpNotFound();
            }
            return View(subjecttopic);
        }

        // GET: subjecttopics/Create
        public ActionResult Create()
        {
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1");
            ViewBag.Topic_id = new SelectList(db.TopicsTables, "Topic_id", "Topic_Name");
            ViewBag.Week_id = new SelectList(db.WeeksTables, "Week_id", "Week_Name");
            return View();
        }

        // POST: subjecttopics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Topic_id,Term_id,Week_id")] subjecttopic subjecttopic)
        {
            if (ModelState.IsValid)
            {
                db.subjecttopics.Add(subjecttopic);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", subjecttopic.Term_id);
            ViewBag.Topic_id = new SelectList(db.TopicsTables, "Topic_id", "Topic_Name", subjecttopic.Topic_id);
            ViewBag.Week_id = new SelectList(db.WeeksTables, "Week_id", "Week_Name", subjecttopic.Week_id);
            return View(subjecttopic);
        }

        // GET: subjecttopics/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subjecttopic subjecttopic = await db.subjecttopics.FindAsync(id);
            if (subjecttopic == null)
            {
                return HttpNotFound();
            }
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", subjecttopic.Term_id);
            ViewBag.Topic_id = new SelectList(db.TopicsTables, "Topic_id", "Topic_Name", subjecttopic.Topic_id);
            ViewBag.Week_id = new SelectList(db.WeeksTables, "Week_id", "Week_Name", subjecttopic.Week_id);
            return View(subjecttopic);
        }

        // POST: subjecttopics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Topic_id,Term_id,Week_id")] subjecttopic subjecttopic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subjecttopic).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", subjecttopic.Term_id);
            ViewBag.Topic_id = new SelectList(db.TopicsTables, "Topic_id", "Topic_Name", subjecttopic.Topic_id);
            ViewBag.Week_id = new SelectList(db.WeeksTables, "Week_id", "Week_Name", subjecttopic.Week_id);
            return View(subjecttopic);
        }

        // GET: subjecttopics/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subjecttopic subjecttopic = await db.subjecttopics.FindAsync(id);
            if (subjecttopic == null)
            {
                return HttpNotFound();
            }
            return View(subjecttopic);
        }

        // POST: subjecttopics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            subjecttopic subjecttopic = await db.subjecttopics.FindAsync(id);
            db.subjecttopics.Remove(subjecttopic);
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
