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
    public class subject_classController : Controller
    {
        private SMISEntities db = new SMISEntities();

        // GET: subject_class
        public async Task<ActionResult> Index()
        {
            var subject_class = db.subject_class.Include(s => s.ClassTable).Include(s => s.SubjectTable);
            return View(await subject_class.ToListAsync());
        }

        public  ActionResult classSubjects(int? id)
        {

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var classsubs = db.subject_class.Where(a => a.Class_id == id).ToList();
                var classid = db.subject_class.Where(a => a.ID == id).Select(a => a.Class_id).FirstOrDefault();
                Session["classid"] = classid;

                return View(classsubs);
            }
            catch(Exception EX)
            {
                TempData["error"] = EX.Message;
                return View();
            }
        }
  
        // GET: subject_class/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subject_class subject_class = await db.subject_class.FindAsync(id);
            if (subject_class == null)
            {
                return HttpNotFound();
            }
            return View(subject_class);
        }

        // GET: subject_class/Create
        public ActionResult Create()
        {
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name");
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name");
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Subject_id,Class_id")] subject_class subject_class)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.subject_class.Add(subject_class);
                    await db.SaveChangesAsync();
                    TempData["success"] = "Subject Added to class";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception EX)
            {
                TempData["error"] = EX.Message;
                
            }

            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", subject_class.Class_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", subject_class.Subject_id);
            return View(subject_class);
        }

        // GET: subject_class/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subject_class subject_class = await db.subject_class.FindAsync(id);
            if (subject_class == null)
            {
                return HttpNotFound();
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", subject_class.Class_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", subject_class.Subject_id);
            return View(subject_class);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Subject_id,Class_id")] subject_class subject_class)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(subject_class).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["success"] = "Subject Changed saved ";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception EX)
            {
                TempData["error"] = EX.Message;

            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", subject_class.Class_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", subject_class.Subject_id);
            return View(subject_class);
        }

        // GET: subject_class/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subject_class subject_class = await db.subject_class.FindAsync(id);
            if (subject_class == null)
            {
                return HttpNotFound();
            }
            return View(subject_class);
        }

        // POST: subject_class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {

                subject_class subject_class = await db.subject_class.FindAsync(id);
                db.subject_class.Remove(subject_class);
                await db.SaveChangesAsync();
                TempData["success"] = "Subject DELETED Succesfully";
                return RedirectToAction("Index");
            }
            catch (Exception EX)
            {
                TempData["error"] = EX.Message;
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
