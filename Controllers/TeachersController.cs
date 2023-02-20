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
    public class TeachersController : Controller
    {
        private SMISEntities db = new SMISEntities();

        // GET: Teachers
        public async Task<ActionResult> Index()
        {
            var teachers = db.Teachers.Include(t => t.ClassTable).Include(t => t.SubjectTable);
            return View(await teachers.ToListAsync());
        }

        // GET: Teachers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = await db.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name");
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name");
            return View();
        }

        // POST: Teachers/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Teacher1,Class_id,Subject_id")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                var boolres = class_Teacher_Exists(teacher.Class_id, teacher.Subject_id, teacher.Teacher1);

                if (boolres == false)
                {
                    try
                    {
                       db.Teachers.Add(teacher);
                        await db.SaveChangesAsync();
                        TempData["success"] = "Teacher assigned Class succesfully";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = ex.Message;

                    }
                }
                else
                {
                    TempData["error"] = "Sorry that class has a teacher assigned to that subject";
                }

            }

            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", teacher.Class_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", teacher.Subject_id);
            return View(teacher);
        }
        [NonAction]
        public bool class_Teacher_Exists(int? classid, int? subjectid, String teahcer)
        {
            var v = db.Teachers.Where(a => a.Teacher1 == teahcer && a.Class_id == classid && a.Subject_id == subjectid).FirstOrDefault();
            return v != null;

        }

        // GET: Teachers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = await db.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", teacher.Class_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", teacher.Subject_id);
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Teacher1,Class_id,Subject_id")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", teacher.Class_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", teacher.Subject_id);
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = await db.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Teacher teacher = await db.Teachers.FindAsync(id);
            db.Teachers.Remove(teacher);
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
