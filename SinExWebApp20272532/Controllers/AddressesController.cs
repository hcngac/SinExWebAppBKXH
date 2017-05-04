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
    public class AddressesController : BaseController
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: Addresses
        public ActionResult Index(string AddressType)
        {
            var addresses = db.Addresses.Include(a => a.ShippingAccount);
            int currentShippingAccountId = GetCurrentShippingAccountId();
            addresses = addresses.Where(t => t.ShippingAccountId == currentShippingAccountId);
            if (AddressType == "RecipientAddress")
            {
                Session["AddressType"] = "RecipientAddress";
                addresses = addresses.Where(s => s.isRecipientAddress == true);
            }
            else if (AddressType == "PickupAddress")
            {
                Session["AddressType"] = "PickupAddress";
                addresses = addresses.Where(s => s.isRecipientAddress == false);
            }
            else if ((string)Session["AddressType"] == "RecipientAddress")
            {
                addresses = addresses.Where(s => s.isRecipientAddress == true);
            }
            else if ((string)Session["AddressType"] == "PickupAddress")
            {
                addresses = addresses.Where(s => s.isRecipientAddress == false);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(addresses.ToList());
        }

        // GET: Addresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // GET: Addresses/Create
        public ActionResult Create()
        {
            if ((string)Session["AddressType"] != "RecipientAddress" && (string)Session["AddressType"] != "PickupAddress")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ShippingAccountId = new SelectList(db.ShippingAccounts, "ShippingAccountId", "Building");

            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AddressId,AddressName,Building,Street,City,ProvinceCode,PostalCode")] Address address)
        {
            if ((string)Session["AddressType"] == "RecipientAddress")
            {
                address.isRecipientAddress = true;
            }
            else if ((string)Session["AddressType"] == "PickupAddress")
            {
                address.isRecipientAddress = false;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                address.ShippingAccountId = GetCurrentShippingAccountId();
                db.Addresses.Add(address);
                db.SaveChanges();
                return RedirectToAction("Index", new { AddressType = (string)Session["AddressType"] });
            }

            ViewBag.ShippingAccountId = new SelectList(db.ShippingAccounts, "ShippingAccountId", "Building", address.ShippingAccountId);
            return View(address);
        }

        // GET: Addresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            if (address.isRecipientAddress)
            {
                Session["AddressType"] = "RecipientAddress";
            }
            else if (!address.isRecipientAddress)
            {
                Session["AddressType"] = "PickupAddress";
            }
            ViewBag.ShippingAccountId = new SelectList(db.ShippingAccounts, "ShippingAccountId", "Building", address.ShippingAccountId);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AddressId,AddressName,Building,Street,City,ProvinceCode,PostalCode")] Address address)
        {
            if ((string)Session["AddressType"] == "RecipientAddress")
            {
                address.isRecipientAddress = true;
            }
            else if ((string)Session["AddressType"] == "PickupAddress")
            {
                address.isRecipientAddress = false;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                address.ShippingAccountId = GetCurrentShippingAccountId();
                db.Entry(address).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { AddressType = (string)Session["AddressType"] });
            }
            ViewBag.ShippingAccountId = new SelectList(db.ShippingAccounts, "ShippingAccountId", "Building", address.ShippingAccountId);
            return View(address);
        }

        // GET: Addresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Address address = db.Addresses.Find(id);
            db.Addresses.Remove(address);
            db.SaveChanges();
            return RedirectToAction("Index", new { AddressType = (string)Session["AddressType"] });
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
