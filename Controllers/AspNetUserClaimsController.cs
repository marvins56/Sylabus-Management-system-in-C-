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
    public class AspNetUserClaimsController : Controller
    {
        private SMISEntities db = new SMISEntities();

        // GET: AspNetUserClaims
        public async Task<ActionResult> Index()
        {
            var aspNetUserClaims = db.AspNetUserClaims.Include(a => a.AspNetUser);
            return View(await aspNetUserClaims.ToListAsync());
        }

        // GET: AspNetUserClaims/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserClaim aspNetUserClaim = await db.AspNetUserClaims.FindAsync(id);
            if (aspNetUserClaim == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUserClaim);
        }

        // GET: AspNetUserClaims/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AspNetUserClaims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserId,ClaimType,ClaimValue")] AspNetUserClaim aspNetUserClaim)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUserClaims.Add(aspNetUserClaim);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUserClaim.UserId);
            return View(aspNetUserClaim);
        }

        // GET: AspNetUserClaims/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserClaim aspNetUserClaim = await db.AspNetUserClaims.FindAsync(id);
            if (aspNetUserClaim == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUserClaim.UserId);
            return View(aspNetUserClaim);
        }

        // POST: AspNetUserClaims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserId,ClaimType,ClaimValue")] AspNetUserClaim aspNetUserClaim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUserClaim).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUserClaim.UserId);
            return View(aspNetUserClaim);
        }

        // GET: AspNetUserClaims/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserClaim aspNetUserClaim = await db.AspNetUserClaims.FindAsync(id);
            if (aspNetUserClaim == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUserClaim);
        }

        // POST: AspNetUserClaims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AspNetUserClaim aspNetUserClaim = await db.AspNetUserClaims.FindAsync(id);
            db.AspNetUserClaims.Remove(aspNetUserClaim);
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
