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
    public class MarksController : Controller
    {
        private SMISEntities2 db = new SMISEntities2();

        // GET: Marks
        public async Task<ActionResult> Index()
        {
            var midtermMarksTables = db.MidtermMarksTables.Include(m => m.ClassTable).Include(m => m.StudentsTable).Include(m => m.SubjectTable).Include(m => m.Term).Include(m => m.Year);
            return View(await midtermMarksTables.ToListAsync());
        }

        // GET: Marks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MidtermMarksTable midtermMarksTable = await db.MidtermMarksTables.FindAsync(id);
            if (midtermMarksTable == null)
            {
                return HttpNotFound();
            }
            return View(midtermMarksTable);
        }

        // GET: Marks/Create
        public ActionResult Create()
        {
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name");
            ViewBag.Student_id = new SelectList(db.StudentsTables, "Student_id", "Student_Name");
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name");
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1");
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Marks_id,Subject_id,Student_id,Class_id,Marks,Status,Term_id,year_Id")] MidtermMarksTable midtermMarksTable)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.MidtermMarksTables.Add(midtermMarksTable);
                    await db.SaveChangesAsync();
                    TempData["success"] = "Marks Added successfully";
                    return RedirectToAction("index", "Class");
                }
            }catch(Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", midtermMarksTable.Class_id);
            ViewBag.Student_id = new SelectList(db.StudentsTables, "Student_id", "Student_Name", midtermMarksTable.Student_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", midtermMarksTable.Subject_id);
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", midtermMarksTable.Term_id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", midtermMarksTable.year_Id);
            return View(midtermMarksTable);
        }

        // GET: Marks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MidtermMarksTable midtermMarksTable = await db.MidtermMarksTables.FindAsync(id);
            if (midtermMarksTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", midtermMarksTable.Class_id);
            ViewBag.Student_id = new SelectList(db.StudentsTables, "Student_id", "Student_Name", midtermMarksTable.Student_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", midtermMarksTable.Subject_id);
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", midtermMarksTable.Term_id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", midtermMarksTable.year_Id);
            return View(midtermMarksTable);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Marks_id,Subject_id,Student_id,Class_id,Marks,Status,Term_id,year_Id")] MidtermMarksTable midtermMarksTable)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(midtermMarksTable).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["success"] = "Marks Added successfully";
                    return RedirectToAction("index", "Class");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", midtermMarksTable.Class_id);
            ViewBag.Student_id = new SelectList(db.StudentsTables, "Student_id", "Student_Name", midtermMarksTable.Student_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", midtermMarksTable.Subject_id);
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", midtermMarksTable.Term_id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", midtermMarksTable.year_Id);
            return View(midtermMarksTable);
        }

        // GET: Marks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MidtermMarksTable midtermMarksTable = await db.MidtermMarksTables.FindAsync(id);
            if (midtermMarksTable == null)
            {
                return HttpNotFound();
            }
            return View(midtermMarksTable);
        }

        // POST: Marks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                MidtermMarksTable midtermMarksTable = await db.MidtermMarksTables.FindAsync(id);
                db.MidtermMarksTables.Remove(midtermMarksTable);
                await db.SaveChangesAsync();
                TempData["success"] = "Marks DELETED successfully";
                return RedirectToAction("Index", "Class");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Class");
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
