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
    public struct Package
    {
        public ServiceType serviceType;
        public PackageType packageType;
        public decimal weight;
        public decimal fee;
    }

    public class ServicePackageFeesController : BaseController
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();



        // GET: ServicePackageFees
        public ActionResult Index(string ServiceType, string PackageType, decimal? Weight, string Currency, bool? clearPackage)
        {

            //Initialization of parameter
            if (clearPackage == null)
            {
                clearPackage = false;
            }
            if (Session["PackageList"] == null)
            {
                Session["PackageList"] = new List<Package>();
            }
            if (ServiceType == "Please Select")
            {
                ServiceType = null;
            }
            if (PackageType == "Please Select")
            {
                PackageType = null;
            }
            if (Currency == "Please Select")
            {
                Currency = null;
            }

            // Get list of ServiceType, PackageType, Currencies
            List<String> stl, ptl, cl;
            stl = db.ServiceTypes.Select(s => s.Type).Distinct().ToList();
            ptl = db.PackageTypes.Select(s => s.Type).Distinct().ToList();
            cl = db.Currencies.Select(s => s.CurrencyCode).Distinct().ToList();
            stl.Insert(0, "Please Select");
            ptl.Insert(0, "Please Select");
            cl.Insert(0, "Please Select");
            ViewBag.serviceTypeList = new SelectList(stl);
            ViewBag.packageTypeList = new SelectList(ptl);
            ViewBag.currencyList = new SelectList(cl);


            ViewBag.ServicePackageFeeCalculated = null;

            // Filter for ServicePackageFees
            var servicePackageFees = db.ServicePackageFees.Include(s => s.PackageType).Include(s => s.ServiceType);
            if (ServiceType != null)
            {
                servicePackageFees = servicePackageFees.Where(s => s.ServiceType.Type == ServiceType);
            }
            if (PackageType != null)
            {
                servicePackageFees = servicePackageFees.Where(s => s.PackageType.Type == PackageType);
            }

            // Determine currency exchange rate
            decimal rate = 1;
            if (Currency != null)
            {
                rate = db.Currencies.Where(s => s.CurrencyCode == Currency).Select(s => s.ExchangeRate).Single();
            }

            // Add package
            var ServicePackageFeeList = servicePackageFees.ToList();
            if (ServicePackageFeeList.Count() == 1 && Weight != null)
            {
                ServicePackageFee spf = ServicePackageFeeList.First();
                decimal PackageFee, FeePerKG, MinimumFee, MaximumWeight;
                FeePerKG = spf.Fee;
                try
                {
                    MaximumWeight = spf.PackageType.PackageTypeSizes.OrderByDescending(s => s.WeightLimit).Select(s => s.WeightLimit).First();
                }
                catch (Exception e)
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
                    PackageFee += 500;
                }

                List<Package> pl = (List<Package>)Session["PackageList"];
                pl.Add(new Package
                {
                    serviceType = spf.ServiceType,
                    packageType = spf.PackageType,
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
                foreach (var x in (List<Package>)Session["PackageList"])
                {
                    ViewBag.TotalFee += x.fee;
                }
            }

            // Compute display currency
            ViewBag.exchangeRate = rate;
            foreach (ServicePackageFee x in ServicePackageFeeList)
            {
                x.Fee = CurrencyExchange(x.Fee, 1, rate);
                x.MinimumFee = CurrencyExchange(x.MinimumFee, 1, rate);
            }

            return View(ServicePackageFeeList);
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
