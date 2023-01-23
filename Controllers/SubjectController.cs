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
    public class SubjectController : Controller
    {
        private SMISEntities2 db = new SMISEntities2();

        // GET: Subject
        public async Task<ActionResult> Index()
        {
            return View(await db.SubjectTables.ToListAsync());
        }

        // GET: Subject/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectTable subjectTable = await db.SubjectTables.FindAsync(id);
            if (subjectTable == null)
            {
                return HttpNotFound();
            }
            return View(subjectTable);
        }

        // GET: Subject/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subject/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Subject_id,Name")] SubjectTable subjectTable)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    db.SubjectTables.Add(subjectTable);
                    await db.SaveChangesAsync();
                    TempData["success"] = "Subject Added successfully";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception Ex)
            {
                TempData["error"] = Ex.Message;
            }
            return View(subjectTable);
        }

        // GET: Subject/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectTable subjectTable = await db.SubjectTables.FindAsync(id);
            if (subjectTable == null)
            {
                return HttpNotFound();
            }
            return View(subjectTable);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Subject_id,Name")] SubjectTable subjectTable)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(subjectTable).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["success"] = "Subject Added successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception Ex)
            {
                TempData["error"] = Ex.Message;
            }
            return View(subjectTable);
        }

        // GET: Subject/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectTable subjectTable = await db.SubjectTables.FindAsync(id);
            if (subjectTable == null)
            {
                return HttpNotFound();
            }
            return View(subjectTable);
        }

        // POST: Subject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                SubjectTable subjectTable = await db.SubjectTables.FindAsync(id);
                db.SubjectTables.Remove(subjectTable);
                await db.SaveChangesAsync();
                TempData["success"] = "Subject DELETED successfully";
                return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                TempData["error"] = Ex.Message;
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
