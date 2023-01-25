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
using SMIS.Models.ViewModel;
using Newtonsoft.Json;
using Twilio.TwiML.Messaging;

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
            Session["returnclassid"] = id;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var classsubs = db.subject_class.Where(a => a.Class_id == id).ToList();
                var classid = db.subject_class.Where(a => a.ID == id).Select(a => a.Class_id).FirstOrDefault();
                Session["classid"] = classid;

                ViewBag.students = db.StudentsTables.Where(a => a.Class_id == classid).Count();
                ViewBag.subjects = db.subject_class.Where(a => a.Class_id == classid).Count();

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

        [NonAction]
        public bool SubjectExixts(int id,int id2)
        {
            var v = db.subject_class.Where(a =>a.Subject_id == id && id2 == a.Class_id).FirstOrDefault();
            return v != null;

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Subject_id,Class_id")] subject_class subject_class)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var id = Convert.ToInt32(Session["returnclassid"]);
                    if(id > 0)
                    {
                        var boolres = SubjectExixts(subject_class.Subject_id, subject_class.Class_id);
                        if(boolres == true)
                        {
                            TempData["error"] = "SUBJECT ALREADY EXISTS IN CLASS";
                        }
                        else
                        {
                            db.subject_class.Add(subject_class);
                            await db.SaveChangesAsync();
                            TempData["success"] = "Subject Added to class";
                            return RedirectToAction("classSubjects", "subject_class", new { id = id });
                        }
                    }
                    else
                    {
                        TempData["info"] = "Error fetching results";
                    }
                    
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
                    var idz = Convert.ToInt32(Session["returnclassid"]);
                    if (idz > 0)
                    {
                        db.Entry(subject_class).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        TempData["success"] = "Subject Changed saved ";
                        return RedirectToAction("classSubjects", "subject_class", new { id = idz });

                    }
                    else
                    {
                        TempData["info"] = "Error fetching results";
                    }

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
        [ChildActionOnly]
        public PartialViewResult chart()
        {
            try
            {
                var classid = Convert.ToInt32(Session["classid"]);
                //var subjectid = Convert.ToInt32(Session["subjectid"]);
                var dta = db.StudentsTables.Where(a => a.Class_id == classid ).Count();
                var subjects = db.subject_class.Where(a => a.Class_id == classid).Count();
                List<DataPoint> dataPoints = new List<DataPoint>();
                List<DataPoint> dataPoints2 = new List<DataPoint>();
                dataPoints.Add(new DataPoint("Students",dta));
                dataPoints.Add(new DataPoint("subjects", subjects));
                ViewBag.datapoints = JsonConvert.SerializeObject(dataPoints);


                return PartialView("chart");
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
                return PartialView("chart", null);
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
