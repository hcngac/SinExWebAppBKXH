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
    public struct DummyPackage
    {
        public ServiceType serviceType;
        public PackageType packageType;
        public PackageTypeSize packageTypeSize;
        public decimal weight;
        public decimal fee;
    }

    public class ServicePackageFeesController : BaseController
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();



        // GET: ServicePackageFees
        public ActionResult Index(string ServiceType, int? PackageTypeSize, decimal? Weight, string Currency, bool? clearPackage)
        {

            //Initialization of parameter
            if (clearPackage == null)
            {
                clearPackage = false;
            }
            if (Session["PackageList"] == null)
            {
                Session["PackageList"] = new List<DummyPackage>();
            }
            if (Session["exchangeRate"] == null)
            {
                Session["exchangeRate"] = (decimal)1;
            }
            if (ServiceType == "")
            {
                ServiceType = null;
            }
            if (Currency == "")
            {
                Currency = null;
            }

            // Get list of ServiceType, PackageType, Currencies

            ViewBag.ServicePackageFeeCalculated = null;

            // Filter for ServicePackageFees
            var servicePackageFees = db.ServicePackageFees.Include(s => s.PackageType).Include(s => s.ServiceType);
            PackageTypeSize currentPackageTypeSize;
            if (ServiceType != null)
            {
                servicePackageFees = servicePackageFees.Where(s => s.ServiceType.Type == ServiceType);
            }
            if (PackageTypeSize != null)
            {
                currentPackageTypeSize = db.PackageTypeSizes.Find(PackageTypeSize);
                servicePackageFees = servicePackageFees.Where(s => s.PackageType.PackageTypeID == currentPackageTypeSize.PackageType.PackageTypeID);
            }

            // Determine currency exchange rate
            if (Currency != null)
            {
                Session["exchangeRate"] = Models.Currency.getCachedList().Where(s => s.CurrencyCode == Currency).Select(s => s.ExchangeRate).Single();
            }

            // Add package
            var ServicePackageFeeList = servicePackageFees.ToList();
            if (ServicePackageFeeList.Count() == 1 && Weight != null && Weight > 0)
            {
                currentPackageTypeSize = db.PackageTypeSizes.Find(PackageTypeSize);
                ServicePackageFee spf = ServicePackageFeeList.First();
                decimal PackageFee, FeePerKG, MinimumFee, MaximumWeight;
                FeePerKG = spf.Fee;
                try
                {
                    MaximumWeight = currentPackageTypeSize.WeightLimit;
                }
                catch (Exception)
                {
                    MaximumWeight = 0;
                }

                MinimumFee = spf.MinimumFee;
                PackageFee = (decimal)Weight * FeePerKG;
                if (PackageFee < MinimumFee)
                {
                    PackageFee = MinimumFee;
                }
                if (Weight > MaximumWeight && MaximumWeight != 0)
                {
                    PackageFee += spf.Penalty;
                }
                if (spf.PackageTypeID == 1)
                {
                    PackageFee = MinimumFee;
                }

                List<DummyPackage> pl = (List<DummyPackage>)Session["PackageList"];
                pl.Add(new DummyPackage
                {
                    serviceType = spf.ServiceType,
                    packageType = spf.PackageType,
                    packageTypeSize = currentPackageTypeSize,
                    weight = (decimal)Weight,
                    fee = PackageFee
                });
            }

            // Clear Package
            if ((bool)clearPackage)
            {
                Session["PackageList"] = null;
            }

            // Calculate Total Fee
            ViewBag.TotalFee = 0;
            if (Session["PackageList"] != null)
            {
                foreach (var x in (List<DummyPackage>)Session["PackageList"])
                {
                    ViewBag.TotalFee += x.fee;
                }
            }

            // Compute display currency
            foreach (ServicePackageFee x in ServicePackageFeeList)
            {
                x.Fee = CurrencyExchange(x.Fee, 1, (decimal)Session["exchangeRate"]);
                x.MinimumFee = CurrencyExchange(x.MinimumFee, 1, (decimal)Session["exchangeRate"]);
            }

            return View(ServicePackageFeeList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePenalty(decimal? newPenalty)
        {
            if (newPenalty == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<ServicePackageFee> ServicePackageFeeList = db.ServicePackageFees.ToList();
            foreach (ServicePackageFee servicePackageFee in ServicePackageFeeList)
            {
                servicePackageFee.Penalty = (decimal)newPenalty;
                db.Entry(servicePackageFee).State = EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: ServicePackageFees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePackageFee servicePackageFee = db.ServicePackageFees.Find(id);
            if (servicePackageFee == null)
            {
                return HttpNotFound();
            }
            return View(servicePackageFee);
        }

        // GET: ServicePackageFees/Create
        public ActionResult Create()
        {
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type");
            ViewBag.ServiceTypeID = new SelectList(db.ServiceTypes, "ServiceTypeID", "Type");
            return View();
        }

        // POST: ServicePackageFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServicePackageFeeID,Fee,MinimumFee,PackageTypeID,ServiceTypeID")] ServicePackageFee servicePackageFee)
        {
            if (ModelState.IsValid)
            {
                db.ServicePackageFees.Add(servicePackageFee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", servicePackageFee.PackageTypeID);
            ViewBag.ServiceTypeID = new SelectList(db.ServiceTypes, "ServiceTypeID", "Type", servicePackageFee.ServiceTypeID);
            return View(servicePackageFee);
        }

        // GET: ServicePackageFees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePackageFee servicePackageFee = db.ServicePackageFees.Find(id);
            if (servicePackageFee == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", servicePackageFee.PackageTypeID);
            ViewBag.ServiceTypeID = new SelectList(db.ServiceTypes, "ServiceTypeID", "Type", servicePackageFee.ServiceTypeID);
            return View(servicePackageFee);
        }

        // POST: ServicePackageFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServicePackageFeeID,Fee,MinimumFee,PackageTypeID,ServiceTypeID")] ServicePackageFee servicePackageFee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicePackageFee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", servicePackageFee.PackageTypeID);
            ViewBag.ServiceTypeID = new SelectList(db.ServiceTypes, "ServiceTypeID", "Type", servicePackageFee.ServiceTypeID);
            return View(servicePackageFee);
        }

        // GET: ServicePackageFees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePackageFee servicePackageFee = db.ServicePackageFees.Find(id);
            if (servicePackageFee == null)
            {
                return HttpNotFound();
            }
            return View(servicePackageFee);
        }

        // POST: ServicePackageFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServicePackageFee servicePackageFee = db.ServicePackageFees.Find(id);
            db.ServicePackageFees.Remove(servicePackageFee);
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
