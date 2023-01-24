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
using System.IO;

namespace SMIS.Controllers
{
    [Authorize]
    public class TopicsController : Controller
    {
        private SMISEntities2 db = new SMISEntities2();

     
public async Task<ActionResult> Index()
        {
            var topicsTables = db.TopicsTables.Include(t => t.ClassTable).Include(t => t.SubjectTable).Include(t => t.Term).Include(t => t.Year).Include(t => t.WeeksTable);
            return View(await topicsTables.ToListAsync());
        }
        [ChildActionOnly]
        public PartialViewResult subjecttopics(int? weekid)
        {
            if (weekid == null)
            {
                TempData["error"] = "ERROR INVALID ID";

            }
            else
            {
                try
                {
                    var subject = Convert.ToInt32(Session["subjectid"]);
                    var classid = Convert.ToInt32(Session["classid"]);

                    var result = db.TopicsTables.Where(a => a.Week_id == weekid && a.Class_id == classid && a.Subject_id == subject).ToList();
                    TempData["info"] = "Fetching Data please Wait..";
                    return PartialView("subjecttopics", result);
                }
                catch (Exception e)
                {
                    TempData["error"] = e.Message;
                }

            }

            return PartialView("subjecttopics");

        }
        [ChildActionOnly]
        public PartialViewResult vidoes(int? weekid)
        {
            if (weekid == null)
            {
                TempData["error"] = "ERROR INVALID ID";
            }
            else
            {
                try
                {
                    var subject = Convert.ToInt32(Session["subjectid"]);
                    var classid = Convert.ToInt32(Session["classid"]);

                    var result = db.TopicsTables.Where(a => a.Week_id == weekid && a.Class_id == classid && a.Subject_id == subject && a.ContentType == "video/mp4").ToList();
                    TempData["info"] = "Fetching Data please Wait..";
                    return PartialView("vidoes", result);
                }
                catch (Exception e)
                {
                    TempData["error"] = e.Message;
                }

            }

            return PartialView("vidoes");

        }
        // GET: Topics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicsTable topicsTable = await db.TopicsTables.FindAsync(id);
            if (topicsTable == null)
            {
                return HttpNotFound();
            }
            return View(topicsTable);
        }

        // GET: Topics/Create
        public ActionResult Create()
        {
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name");
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name");
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1");
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1");
            ViewBag.Week_id = new SelectList(db.WeeksTables, "Week_id", "Week_Name");
            return View();
        }

        [NonAction]
        public bool TopicExists(int id, int id2, int id3,string topicname)
        {
            var v = db.TopicsTables.Where(a => a.Subject_id == id && id2 == a.Class_id && a.Week_id == id3 && a.Topic_Name.ToLower() == topicname.ToLower() ).FirstOrDefault();
            return v != null;

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Topic_id,Topic_Name,Class_id,IsComplete,DateTime,Subject_id,Term_id,year_Id,Week_id,Overview,File,ContentType,Data")] TopicsTable topicsTable, HttpPostedFileBase postedFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // db.TopicsTables.Add(topicsTable);

                    var ids = Convert.ToInt32(Session["redirectid"]);
                    if(ids > 0) {

                        var boolres = TopicExists(topicsTable.Subject_id, topicsTable.Class_id, topicsTable.Week_id, topicsTable.Topic_Name);
                        if(boolres == true)
                        {
                            TempData["error"] = "TOPIC EXIXTS, kindly make changes to exixting topic";
                        }
                        else
                        {
                            if(topicsTable.Class_id != (Convert.ToInt32(Session["classid"])) && topicsTable.Subject_id !=(Convert.ToInt32(Session["subjectid"])) )
                                {
                                TempData["error"] = "Kindly select exact class changes are being made";

                            }
                            else
                            {

                                byte[] bytes;
                                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                                {
                                    bytes = br.ReadBytes(postedFile.ContentLength);
                                }

                                db.TopicsTables.Add(new TopicsTable
                                {
                                    File = Path.GetFileName(postedFile.FileName),
                                    ContentType = postedFile.ContentType,
                                    Data = bytes,
                                    Topic_Name = topicsTable.Topic_Name,
                                    Class_id = topicsTable.Class_id,
                                    IsComplete = topicsTable.IsComplete,

                                    Subject_id = topicsTable.Subject_id,
                                    Term_id = topicsTable.Term_id,
                                    year_Id = topicsTable.year_Id,
                                    Week_id = topicsTable.Week_id,
                                    Overview = topicsTable.Overview,
                                    DateTime = DateTime.Now

                                });

                                await db.SaveChangesAsync();
                                return RedirectToAction("Detail_Dashboard", "Users", new { id = ids });
                            }

                        }

                        
                    }
                    else
                    {
                        TempData["Info"] = "Error fetching data";
                    }
                   
                }
            }catch(Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", topicsTable.Class_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", topicsTable.Subject_id);
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", topicsTable.Term_id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", topicsTable.year_Id);
            ViewBag.Week_id = new SelectList(db.WeeksTables, "Week_id", "Week_Name", topicsTable.Week_id);
            return View(topicsTable);
        }
        [HttpGet]
        public FileResult DownloadFile(int? fileId)
        {
            TopicsTable file = db.TopicsTables.ToList().Find(p => p.Topic_id == fileId.Value);
            return File(file.Data, file.ContentType, file.File);
        }
        // GET: Topics/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicsTable topicsTable = await db.TopicsTables.FindAsync(id);
            if (topicsTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", topicsTable.Class_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", topicsTable.Subject_id);
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", topicsTable.Term_id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", topicsTable.year_Id);
            ViewBag.Week_id = new SelectList(db.WeeksTables, "Week_id", "Week_Name", topicsTable.Week_id);
            return View(topicsTable);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Topic_id,Topic_Name,Class_id,IsComplete,DateTime,Subject_id,Term_id,year_Id,Week_id,Overview,File,ContentType,Data")] TopicsTable topicsTable)
        {
            if (ModelState.IsValid)
            {
                var ids = Convert.ToInt32(Session["redirectid"]);
                if (ids > 0)
                {
                    try
                    {
                        if (topicsTable.Class_id != (Convert.ToInt32(Session["classid"])) && topicsTable.Subject_id != (Convert.ToInt32(Session["subjectid"])))
                        {
                            TempData["error"] = "Kindly select exact class changes are being made";

                        }
                        else
                        {
                            db.Entry(topicsTable).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            return RedirectToAction("Detail_Dashboard", "Users", new { id = ids });
                        }
                    }catch(Exception ex){
                        TempData["error"] = ex.Message;
                    }
                    }
            }
            ViewBag.Class_id = new SelectList(db.ClassTables, "Class_id", "Class_Name", topicsTable.Class_id);
            ViewBag.Subject_id = new SelectList(db.SubjectTables, "Subject_id", "Name", topicsTable.Subject_id);
            ViewBag.Term_id = new SelectList(db.Terms, "Term_Id", "term1", topicsTable.Term_id);
            ViewBag.year_Id = new SelectList(db.Years, "year_Id", "year1", topicsTable.year_Id);
            ViewBag.Week_id = new SelectList(db.WeeksTables, "Week_id", "Week_Name", topicsTable.Week_id);
            return View(topicsTable);
        }

        // GET: Topics/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicsTable topicsTable = await db.TopicsTables.FindAsync(id);
            if (topicsTable == null)
            {
                return HttpNotFound();
            }
            return View(topicsTable);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TopicsTable topicsTable = await db.TopicsTables.FindAsync(id);
            db.TopicsTables.Remove(topicsTable);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public PartialViewResult topicName(int topicsid)
        {
                var result = db.TopicsTables.Where(a => a.Topic_id == topicsid).Select(a => a.Topic_Name).FirstOrDefault();
                return PartialView("topicName", result);
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
