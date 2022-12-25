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
    public class WeeksController : Controller
    {
        private SMISEntities db = new SMISEntities();

        // GET: Weeks
        public async Task<ActionResult> Index()
        {
            return View(await db.WeeksTables.ToListAsync());
        }

        // GET: Weeks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeeksTable weeksTable = await db.WeeksTables.FindAsync(id);
            if (weeksTable == null)
            {
                return HttpNotFound();
            }
            return View(weeksTable);
        }

        // GET: Weeks/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Week_id,Week_Name")] WeeksTable weeksTable)
        {
            if (ModelState.IsValid)
            {
                db.WeeksTables.Add(weeksTable);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(weeksTable);
        }

        // GET: Weeks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeeksTable weeksTable = await db.WeeksTables.FindAsync(id);
            if (weeksTable == null)
            {
                return HttpNotFound();
            }
            return View(weeksTable);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Week_id,Week_Name")] WeeksTable weeksTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(weeksTable).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(weeksTable);
        }

        // GET: Weeks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeeksTable weeksTable = await db.WeeksTables.FindAsync(id);
            if (weeksTable == null)
            {
                return HttpNotFound();
            }
            return View(weeksTable);
        }

        // POST: Weeks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WeeksTable weeksTable = await db.WeeksTables.FindAsync(id);
            db.WeeksTables.Remove(weeksTable);
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
