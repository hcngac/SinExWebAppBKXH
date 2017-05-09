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
    public class TrackingSystemRecordsController : Controller
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        // GET: TrackingSystemRecords
        public ActionResult Index()
        {
            return View(db.TrackingSystemRecords.ToList());
        }

        // GET: TrackingSystemRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackingSystemRecord trackingSystemRecord = db.TrackingSystemRecords.Find(id);
            if (trackingSystemRecord == null)
            {
                return HttpNotFound();
            }
            return View(trackingSystemRecord);
        }

        // GET: TrackingSystemRecords/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrackingSystemRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrackingSystemRecordId,WaybillId,DateTimeOfRecord,Activity,Location,Remarks,DeliveredTo,DeliveredAt,Status")] TrackingSystemRecord trackingSystemRecord)
        {
            if (ModelState.IsValid)
            {
                db.TrackingSystemRecords.Add(trackingSystemRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trackingSystemRecord);
        }

        // GET: TrackingSystemRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackingSystemRecord trackingSystemRecord = db.TrackingSystemRecords.Find(id);
            if (trackingSystemRecord == null)
            {
                return HttpNotFound();
            }
            return View(trackingSystemRecord);
        }

        // POST: TrackingSystemRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrackingSystemRecordId,WaybillId,DateTimeOfRecord,Activity,Location,Remarks,DeliveredTo,DeliveredAt,Status")] TrackingSystemRecord trackingSystemRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trackingSystemRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trackingSystemRecord);
        }

        // GET: TrackingSystemRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackingSystemRecord trackingSystemRecord = db.TrackingSystemRecords.Find(id);
            if (trackingSystemRecord == null)
            {
                return HttpNotFound();
            }
            return View(trackingSystemRecord);
        }


        // POST: TrackingSystemRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrackingSystemRecord trackingSystemRecord = db.TrackingSystemRecords.Find(id);
            db.TrackingSystemRecords.Remove(trackingSystemRecord);
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
