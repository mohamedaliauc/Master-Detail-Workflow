using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterDetail.Models;

namespace MasterDetail.Controllers
{
    public class LaborsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Labors/
        public async Task<ActionResult> Index()
        {
            var labors = db.Labors.Include(l => l.WorkOrder);
            return View(await labors.ToListAsync());
        }

        // GET: /Labors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Labor labor = await db.Labors.FindAsync(id);
            if (labor == null)
            {
                return HttpNotFound();
            }
            return View(labor);
        }

        // GET: /Labors/Create
        public ActionResult Create()
        {
            ViewBag.WorkOrderId = new SelectList(db.WorkOrders, "WorkOrderId", "Description");
            return View();
        }

        // POST: /Labors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="LaborId,WorkOrderId,ServiceItemCode,ServiceItemName,LaborHours,Rate,ExtendedPrice,Notes")] Labor labor)
        {
            if (ModelState.IsValid)
            {
                db.Labors.Add(labor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.WorkOrderId = new SelectList(db.WorkOrders, "WorkOrderId", "Description", labor.WorkOrderId);
            return View(labor);
        }

        // GET: /Labors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Labor labor = await db.Labors.FindAsync(id);
            if (labor == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkOrderId = new SelectList(db.WorkOrders, "WorkOrderId", "Description", labor.WorkOrderId);
            return View(labor);
        }

        // POST: /Labors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="LaborId,WorkOrderId,ServiceItemCode,ServiceItemName,LaborHours,Rate,ExtendedPrice,Notes")] Labor labor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(labor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.WorkOrderId = new SelectList(db.WorkOrders, "WorkOrderId", "Description", labor.WorkOrderId);
            return View(labor);
        }

        // GET: /Labors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Labor labor = await db.Labors.FindAsync(id);
            if (labor == null)
            {
                return HttpNotFound();
            }
            return View(labor);
        }

        // POST: /Labors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Labor labor = await db.Labors.FindAsync(id);
            db.Labors.Remove(labor);
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
