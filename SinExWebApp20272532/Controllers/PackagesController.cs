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
    public class PackagesController : BaseController
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: Packages
        public ActionResult Index()
        {
            if (Session["HandlingWaybillId"] == null)
                return View(new List<Package>());

            int waybillId = (int)Session["HandlingWaybillId"];
            int ShippingAccountId = -1, SenderShippingAccountId = -2;
            try
            {
                ShippingAccountId = db.ShippingAccounts.Where(s => s.UserName == User.Identity.Name).Select(s => s.ShippingAccountId).Single();
                SenderShippingAccountId = db.Shipments.Where(s => s.WaybillId == waybillId).Single().SenderId;

            }
            catch (Exception)
            {
                return View(new List<Package>());
            }
            if (ShippingAccountId != SenderShippingAccountId)
                return View(new List<Package>());

            var packages = db.Packages.Where(s => s.WaybillId == waybillId).Include(p => p.PackageTypeSize.PackageType).Include(p => p.Shipment).ToList();
            foreach (Package package in packages)
            {
                package.ValueOfContent = CurrencyExchange(package.ValueOfContent, db.Currencies.Find(package.ContentCurrency).ExchangeRate, Session["exchangeRate"] == null ? 1 : (decimal)Session["exchangeRate"]);
            }
            return View(packages);
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
            package.ValueOfContent = CurrencyExchange(package.ValueOfContent, db.Currencies.Find(package.ContentCurrency).ExchangeRate, (decimal)Session["exchangeRate"]);
            return View(package);
        }

        // GET: Packages/Create
        public ActionResult Create()
        {
            if (Session["HandlingWaybillId"] != null)
            {

                ViewBag.PackageTypeSizeId = PackageTypeSize.GetSelectList();
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Packages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PackageId,PackageTypeSizeId,Description,ValueOfContent,ContentCurrency,EstimatedWeight")] Package package)
        {
            if (ModelState.IsValid && Session["HandlingWaybillId"] != null)
            {
                package.WaybillId = (int)Session["HandlingWaybillId"];
                db.Packages.Add(package);
                db.SaveChanges();
                Shipment relatedShipment = db.Shipments.Find(package.WaybillId);
                relatedShipment.NumberOfPackages = relatedShipment.Packages.Count();
                db.Entry(relatedShipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Packages");
            }

            if (Session["HandlingWaybillId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.PackageTypeSizeId = PackageTypeSize.GetSelectList(package.PackageTypeSizeId);
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
            ViewBag.PackageTypeSizeId = PackageTypeSize.GetSelectList(package.PackageTypeSizeId);
            return View(package);
        }

        // POST: Packages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PackageId,PackageTypeSizeId,Description,ValueOfContent,ContentCurrency,EstimatedWeight,Weight")] Package package)
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
            ViewBag.PackageTypeSizeId = PackageTypeSize.GetSelectList(package.PackageTypeSizeId);
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
            package.ValueOfContent = CurrencyExchange(package.ValueOfContent, db.Currencies.Find(package.ContentCurrency).ExchangeRate, (decimal)Session["exchangeRate"]);
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
            package.Shipment.NumberOfPackages = package.Shipment.Packages.Count();
            db.Entry(package.Shipment).State = EntityState.Modified;
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
