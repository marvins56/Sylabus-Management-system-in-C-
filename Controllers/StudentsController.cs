﻿using System;
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
            try
            {
                if (ModelState.IsValid)
                {
                    var boolresult = StudentExists(studentsTable.Student_Name);
                    if(boolresult == true)
                    {
                        TempData["error"] = "Student Already Exixts";

                    }
                    else
                    {
                        db.StudentsTables.Add(studentsTable);
                        await db.SaveChangesAsync();
                        TempData["success"] = "Student Created Successfully";
                        return RedirectToAction("Index", "Class");
                    }
                   
                }
            }catch(Exception e)
            {
                TempData["error"] = e.Message;

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
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(studentsTable).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["success"] = "Student Chnages Saved Successfully";
                    return RedirectToAction("Index","Class");
                }
            }catch (Exception e)
            {
                TempData["error"] = e.Message;
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
            try
            {
                StudentsTable studentsTable = await db.StudentsTables.FindAsync(id);
                db.StudentsTables.Remove(studentsTable);
                await db.SaveChangesAsync();
                TempData["success"] = "DELETED successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index");
            }
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
        [ChildActionOnly]
        public PartialViewResult Student_marks(int? student_id)
        {
           
            if (student_id == null)
            {
                TempData["error"] = "ERROR LOADING DATA";
            }

           
            var get_students = get_student(student_id);
            var student_marks = Get_student_marks_info(student_id);

            var studentMarks = new StudentMarks();

            studentMarks.stdntMarks = student_marks;
            studentMarks.student = get_students;

            return PartialView("Student_marks", studentMarks);
        }

        public List<StudentsTable> get_student(int? id)
        {
            try
            {
                var studnts = db.StudentsTables.Where(a => a.Student_id == id).ToList();
                return (studnts);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return null;
            }
        }
        public List<MidtermMarksTable> Get_student_marks_info(int? id)
        {
            try
            {
                var std_marks = db.MidtermMarksTables.Where(a => a.Student_id == id).ToList();
                return std_marks;
            }catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return null;
            }
        }
        [NonAction]
        public bool StudentExists(String stName)
        {
            var v = db.StudentsTables.Where(a => a.Student_Name == stName).FirstOrDefault();
            return v != null;

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
