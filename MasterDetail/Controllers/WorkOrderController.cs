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
    public class WorkOrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /WorkOrder/
        public async Task<ActionResult> Index()
        {
            var workorders = db.WorkOrders.Include(w => w.Customer);
            return View(await workorders.ToListAsync());
        }

        // GET: /WorkOrder/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workorder = await db.WorkOrders.FindAsync(id);
            if (workorder == null)
            {
                return HttpNotFound();
            }
            return View(workorder);
        }

        // GET: /WorkOrder/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "AccountNumber");
            return View();
        }

        // POST: /WorkOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="WorkOrderId,CustomerId,OrderDateTime,TargetDateTime,DropDeadDateTime,Description,WorkOrderStatus,Total,CertificationRequirements")] WorkOrder workorder)
        {
            if (ModelState.IsValid)
            {
                db.WorkOrders.Add(workorder);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "AccountNumber", workorder.CustomerId);
            return View(workorder);
        }

        // GET: /WorkOrder/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workorder = await db.WorkOrders.FindAsync(id);
            if (workorder == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "AccountNumber", workorder.CustomerId);
            return View(workorder);
        }

        // POST: /WorkOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="WorkOrderId,CustomerId,OrderDateTime,TargetDateTime,DropDeadDateTime,Description,WorkOrderStatus,Total,CertificationRequirements")] WorkOrder workorder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workorder).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "AccountNumber", workorder.CustomerId);
            return View(workorder);
        }

        // GET: /WorkOrder/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workorder = await db.WorkOrders.FindAsync(id);
            if (workorder == null)
            {
                return HttpNotFound();
            }
            return View(workorder);
        }

        // POST: /WorkOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WorkOrder workorder = await db.WorkOrders.FindAsync(id);
            db.WorkOrders.Remove(workorder);
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
