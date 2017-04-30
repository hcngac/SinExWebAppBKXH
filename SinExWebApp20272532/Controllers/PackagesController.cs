using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20272532.Models;

namespace SinExWebApp20272532.Controllers
{
    public class PackagesController : Controller
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: Packages
        public ActionResult Index()
        {
            int waybillId = (int)Session["HandlingWaybillId"];

            if (waybillId == null)
                return View(new List<Package>());

            try
            {
                int ShippingAccountId = db.ShippingAccounts.Where(s => s.UserName == User.Identity.Name).Select(s => s.ShippingAccountId).Single();
                int ShipmentShipmentAccountId = db.Shipments.Where(s => s.WaybillId == waybillId).Single().SenderId;
                if (ShippingAccountId != ShipmentShipmentAccountId)
                    return View(new List<Package>());

                var packages = db.Packages.Where(s => s.WaybillId == waybillId).Include(p => p.PackageType).Include(p => p.Shipment);
                return View(packages.ToList());
            }
            catch (Exception)
            {
                return View(new List<Package>());
            }
        }

        // GET: Packages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // GET: Packages/Create
        public ActionResult Create()
        {
            if (Session["HandlingWaybillId"] != null)
            {

                ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type");
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Packages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PackageId,PackageTypeID,Description,ValueOfContent,EstimatedWeight")] Package package)
        {
            if (ModelState.IsValid && Session["HandlingWaybillId"] != null)
            {
                package.WaybillId = (int)Session["HandlingWaybillId"];
                db.Packages.Add(package);
                db.SaveChanges();
                return RedirectToAction("Index", "Packages");
            }

            if (Session["HandlingWaybillId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", package.PackageTypeID);
            return View(package);
        }

        // GET: Packages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["HandlingWaybillId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", package.PackageTypeID);
            return View(package);
        }

        // POST: Packages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PackageId,PackageTypeID,Description,ValueOfContent,EstimatedWeight,Weight")] Package package)
        {
            if (ModelState.IsValid && Session["HandlingWaybillId"] != null)
            {
                package.WaybillId = (int)Session["HandlingWaybillId"];
                db.Entry(package).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (Session["HandlingWaybillId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", package.PackageTypeID);
            return View(package);
        }

        // GET: Packages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // POST: Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Package package = db.Packages.Find(id);
            db.Packages.Remove(package);
            db.SaveChanges();
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
