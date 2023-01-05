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
    public class ClassController : Controller
    {
        private SMISEntities db = new SMISEntities();

        // GET: Class
        public async Task<ActionResult> Index()
        {
            
            return View(await db.ClassTables.ToListAsync());
        }
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ClassTable classTable = await db.ClassTables.FindAsync(id);
                if (classTable == null)
                {
                    return HttpNotFound();
                }
                return View(classTable);
            }catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return View();
            }
        }

        // GET: Class/Create
        public ActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Class_id,Class_Name")] ClassTable classTable)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ClassTables.Add(classTable);
                    await db.SaveChangesAsync();
                    TempData["success"] = "Class Added Successfuly";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex){
                TempData["error"] = ex.Message;
            }

            return View(classTable);
        }

        // GET: Class/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassTable classTable = await db.ClassTables.FindAsync(id);
            if (classTable == null)
            {
                return HttpNotFound();
            }
            return View(classTable);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Class_id,Class_Name")] ClassTable classTable)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(classTable).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["success"] = "Changes Applied Successfuly";
                    return RedirectToAction("Index");
                }
                return View(classTable);
            }catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return View();
            }
            
        }

        // GET: Class/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassTable classTable = await db.ClassTables.FindAsync(id);
            if (classTable == null)
            {
                return HttpNotFound();
            }
            return View(classTable);
        }
        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ClassTable classTable = await db.ClassTables.FindAsync(id);
                db.ClassTables.Remove(classTable);
                await db.SaveChangesAsync();
                TempData["success"] = "Class DELETED successfully";
                return RedirectToAction("Index");
            }catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult studentsPerclass(int? id)
        {
            try
            {
                var studentyear = DateTime.Now.Year;
                var students = db.StudentsTables.Where(a => a.Class_id == id /*&& a.Year.Equals(studentyear)*/).ToList();
                return View(students);
            }catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return View();
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
