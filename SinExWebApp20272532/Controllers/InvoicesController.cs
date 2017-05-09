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
    public class InvoicesController : Controller
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: Invoices
        public ActionResult Index(int? ShippingAccountId, DateTime? DateFrom, DateTime? DateTo, string sortOrder, int? currentShippingAccountId)
        {
            ViewBag.CurrentSort = sortOrder;

            // Retain search conditions for sorting.
            if (User.IsInRole("Customer"))
            {
                ShippingAccountId = db.ShippingAccounts.Where(s => s.UserName == User.Identity.Name).Select(s => s.ShippingAccountId).Single();
            }
            else if (User.IsInRole("Employee"))
            {
                if (ShippingAccountId == null)
                {
                    ShippingAccountId = currentShippingAccountId;
                }
            }
            ViewBag.CurrentShippingAccountId = ShippingAccountId;

            var invoiceQuery = from s in db.Invoices select s;

            if (ShippingAccountId != null)
            {
                // TODO: Construct the LINQ query to retrive only the shipments for the specified shipping account id.
                invoiceQuery = db.Invoices.Where(s => s.ShippingAccountId == ShippingAccountId);

                // Code for date range search
                if (DateFrom != null)
                {
                    invoiceQuery = invoiceQuery.Where(s => s.ShipDate >= DateFrom);
                }
                if (DateTo != null)
                {
                    invoiceQuery = invoiceQuery.Where(s => s.ShipDate <= DateTo);
                }

                // Code for sorting.
                ViewBag.WaybillIdSortParm = string.IsNullOrEmpty(sortOrder) ? "waybillId" : "";
                ViewBag.ShipDateSortParm = sortOrder == "shipDate" ? "shipDate_desc" : "shipDate";
                ViewBag.RecipientNameSortParm = sortOrder == "recipientName" ? "recipientName_desc" : "recipientName";
                ViewBag.OriginSortParm = sortOrder == "origin" ? "origin_desc" : "origin";
                ViewBag.DestinationSortParm = sortOrder == "destination" ? "destination_desc" : "destination";
                ViewBag.ServiceTypeSortParm = sortOrder == "serviceType" ? "serviceType_desc" : "serviceType";
                ViewBag.TotalInvoiceAmountSortParm = sortOrder == "totalInvoiceAmount" ? "totalInvoiceAmount_desc" : "totalInvoiceAmount";
                switch (sortOrder)
                {
                    case "waybillId":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.WaybillId);
                        break;
                    case "shipDate":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.ShipDate);
                        break;
                    case "shippedDate_desc":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.ShipDate);
                        break;
                    case "recipientName":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.RecipientName);
                        break;
                    case "recipientName_desc":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.RecipientName);
                        break;
                    case "origin":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.Origin);
                        break;
                    case "origin_desc":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.Origin);
                        break;
                    case "destination":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.Destination);
                        break;
                    case "destination_desc":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.Destination);
                        break;
                    case "serviceType":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.ServiceType);
                        break;
                    case "serviceType_desc":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.ServiceType);
                        break;
                    case "totalInvoiceAmount":
                        invoiceQuery = invoiceQuery.OrderBy(s => s.ServiceType);
                        break;
                    case "totalInvoiceAmount_desc":
                        invoiceQuery = invoiceQuery.OrderByDescending(s => s.ServiceType);
                        break;
                    default:
                        invoiceQuery = invoiceQuery.OrderBy(s => s.WaybillId);
                        break;
                }
            }
            else
            {
                // Return an empty result if no shipping account id has been selected.
                return View(new List<Invoice>());
            }

            return View(invoiceQuery.ToList());
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceId,WaybillId,ShipDate,RecipientName,Origin,Destination,ServiceType,InvoiceAmount")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceId,WaybillId,ShipDate,RecipientName,Origin,Destination,ServiceType,InvoiceAmount")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
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
