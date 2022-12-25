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
    public class StudentsController : Controller
    {
        private SMISEntities db = new SMISEntities();

        // GET: Students
        public async Task<ActionResult> Index()
        {
            var studentsTables = db.StudentsTables.Include(s => s.ClassTable).Include(s => s.Term).Include(s => s.Year);
            return View(await studentsTables.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentsTable studentsTable = await db.StudentsTables.FindAsync(id);
            if (studentsTable == null)
            {
                return HttpNotFound();
            }
            return View(studentsTable);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name");
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1");
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Student_id,Student_Name,Class_id,Term_id,year_Id")] StudentsTable studentsTable)
        {
            if (ModelState.IsValid)
            {
                db.StudentsTables.Add(studentsTable);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", studentsTable.Class_id);
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", studentsTable.Term_id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", studentsTable.year_Id);
            return View(studentsTable);
        }

        // GET: Students/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentsTable studentsTable = await db.StudentsTables.FindAsync(id);
            if (studentsTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", studentsTable.Class_id);
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", studentsTable.Term_id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", studentsTable.year_Id);
            return View(studentsTable);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Student_id,Student_Name,Class_id,Term_id,year_Id")] StudentsTable studentsTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentsTable).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", studentsTable.Class_id);
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", studentsTable.Term_id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", studentsTable.year_Id);
            return View(studentsTable);
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentsTable studentsTable = await db.StudentsTables.FindAsync(id);
            if (studentsTable == null)
            {
                return HttpNotFound();
            }
            return View(studentsTable);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            StudentsTable studentsTable = await db.StudentsTables.FindAsync(id);
            db.StudentsTables.Remove(studentsTable);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult studentMarks(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentmarks = db.MidtermMarksTables.Where(a => a.Student_id == id).ToList();
            return PartialView("studentMarks",studentmarks);
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
