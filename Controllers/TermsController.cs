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
    public class TermsController : Controller
    {
        private SMISEntities     db = new SMISEntities();

        // GET: Terms
        public async Task<ActionResult> Index()
        {
            return View(await db.Terms.ToListAsync());
        }

        // GET: Terms/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Term term = await db.Terms.FindAsync(id);
            if (term == null)
            {
                return HttpNotFound();
            }
            return View(term);
        }

        // GET: Terms/Create
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Term_Id,term1")] Term term)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Terms.Add(term);
                    await db.SaveChangesAsync();
                    TempData["success"] = "Term Added Successfully";
                    return RedirectToAction("Index");
                }
            }catch(Exception ex) {
                TempData["error"] = ex.Message;      
            }

            return View(term);
        }

        // GET: Terms/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Term term = await db.Terms.FindAsync(id);
            if (term == null)
            {
                return HttpNotFound();
            }
            return View(term);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Term_Id,term1")] Term term)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(term).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["success"] = "Changes Saved Successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return View(term);
        }

        // GET: Terms/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Term term = await db.Terms.FindAsync(id);
            if (term == null)
            {
                return HttpNotFound();
            }
            return View(term);
        }

        // POST: Terms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Term term = await db.Terms.FindAsync(id);
            db.Terms.Remove(term);
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
