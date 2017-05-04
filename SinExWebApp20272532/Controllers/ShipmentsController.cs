using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20272532.Models;
using SinExWebApp20272532.ViewModels;
using X.PagedList;
using System.Data.Entity.Infrastructure;

namespace SinExWebApp20272532.Controllers
{
    public class ShipmentsController : BaseController
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        private void GenerateProvinceCodeList()
        {
            SelectList proviceCode = new SelectList(new List<SelectListItem> {
                new SelectListItem {Value = "AH",Text = "Anhui Province"                          , Selected = false  },
                new SelectListItem {Value = "BJ",Text = "Beijing Municipality"                    , Selected = false  },
                new SelectListItem {Value = "CQ",Text = "Chongqing Municipality"                  , Selected = false  },
                new SelectListItem {Value = "FJ",Text = "Fujian Province"                         , Selected = false  },
                new SelectListItem {Value = "GD",Text = "Guangdong Province"                      , Selected = false  },
                new SelectListItem {Value = "GS",Text = "Gansu Province"                          , Selected = false  },
                new SelectListItem {Value = "GX",Text = "Guangxi Zhuang Autonomous Region"        , Selected = false  },
                new SelectListItem {Value = "GZ",Text = "Guizhou Province"                        , Selected = false  },
                new SelectListItem {Value = "HA",Text = "Henan Province"                          , Selected = false  },
                new SelectListItem {Value = "HB",Text = "Hubei Province"                          , Selected = false  },
                new SelectListItem {Value = "HE",Text = "Hebei Province"                          , Selected = false  },
                new SelectListItem {Value = "HI",Text = "Hainan Province"                         , Selected = false  },
                new SelectListItem {Value = "HK",Text = "Hong Kong Special Administrative Region" , Selected = false  },
                new SelectListItem {Value = "HL",Text = "Heilongjiang Province"                   , Selected = false  },
                new SelectListItem {Value = "HN",Text = "Hunan Province"                          , Selected = false  },
                new SelectListItem {Value = "JL",Text = "Jilin Province"                          , Selected = false  },
                new SelectListItem {Value = "JS",Text = "Jiangsu Province"                        , Selected = false  },
                new SelectListItem {Value = "JX",Text = "Jiangxi Province"                        , Selected = false  },
                new SelectListItem {Value = "LN",Text = "Liaoning Province"                       , Selected = false  },
                new SelectListItem {Value = "MC",Text = "Macau Special Administrative Region"     , Selected = false  },
                new SelectListItem {Value = "NM",Text = "Inner Mongolia Autonomous Region"        , Selected = false  },
                new SelectListItem {Value = "NX",Text = "Ningxia Hui Autonomous Region"           , Selected = false  },
                new SelectListItem {Value = "QH",Text = "Qinghai Province"                        , Selected = false  },
                new SelectListItem {Value = "SC",Text = "Sichuan Province"                        , Selected = false  },
                new SelectListItem {Value = "SD",Text = "Shandong Province"                       , Selected = false  },
                new SelectListItem {Value = "SH",Text = "Shanghai Municipality"                   , Selected = false  },
                new SelectListItem {Value = "SN",Text = "Shaanxi Province"                        , Selected = false  },
                new SelectListItem {Value = "SX",Text = "Shanxi Province"                         , Selected = false  },
                new SelectListItem {Value = "TJ",Text = "Tianjin Municipality"                    , Selected = false  },
                new SelectListItem {Value = "TW",Text = "Taiwan Province"                         , Selected = false  },
                new SelectListItem {Value = "XJ",Text = "Xinjiang Uyghur Autonomous Region"       , Selected = false  },
                new SelectListItem {Value = "XZ",Text = "Tibet Autonomous Region"                 , Selected = false  },
                new SelectListItem {Value = "YN",Text = "Yunnan Province"                         , Selected = false  },
                new SelectListItem {Value = "ZJ",Text = "Zhejiang Province"                       , Selected = false  }
            }, "Value", "Text");
            ViewBag.ProvinceCodeList = proviceCode;
        }
        private void GenerateAddressList()
        {
            int ShippingAccountId = db.ShippingAccounts.Where(s => s.UserName == User.Identity.Name).Select(s => s.ShippingAccountId).Single();
            ViewBag.RecipientAddressList = Address.GetSelectList(ShippingAccountId, true);
            ViewBag.PickupAddressList = Address.GetSelectList(ShippingAccountId, false);
        }
        private Address GetAddressEntity(int AddressId)
        {
            return db.Addresses.Find(AddressId);
        }
        private bool isPersonalShippingAccount(int ShippingAccountId)
        {
            var shippingAccount = db.ShippingAccounts.Find(ShippingAccountId);
            try
            {
                var personalShippingAccount = (PersonalShippingAccount)shippingAccount;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        // GET: Shipments
        public ActionResult Index()
        {
            return View(db.Shipments.Where(s => s.Status != "Cancelled").ToList());
        }

        public ActionResult CancelShipment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            if (shipment.Status == "Cancelled")
            {
                ViewBag.CancelShipmentError = "Shipment already cancelled";
            }
            else if (!shipment.isConfirmed)
            {
                ViewBag.CancelShipmentError = "Shipment not confirmed yet.";
            }
            else if (db.TrackingSystemRecords.Where(s => s.WaybillId == id).Count() != 0)
            {
                ViewBag.CancelShipmentError = "Shipment already picked up.";
            }

            Session["HandlingWaybillId"] = id;
            ViewBag.DeliveryAddressEntity = GetAddressEntity(shipment.DeliveryAddress);
            ViewBag.PickupAddressEntity = GetAddressEntity(shipment.PickupAddress);
            if (isPersonalShippingAccount(shipment.SenderId))
            {
                ViewBag.SenderName = ((PersonalShippingAccount)shipment.Sender).FirstName + " " + ((PersonalShippingAccount)shipment.Sender).LastName;
            }
            else
            {
                ViewBag.SenderName = ((BusinessShippingAccount)shipment.Sender).ContactPersonName;
            }
            return View(shipment);
        }

        [HttpPost, ActionName("CancelShipment")]
        [ValidateAntiForgeryToken]
        public ActionResult CancellingShipment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            if (shipment.Status == "Cancelled")
            {
                ViewBag.CancelShipmentError = "Shipment already cancelled";
            }
            else if (!shipment.isConfirmed)
            {
                ViewBag.CancelShipmentError = "Shipment not confirmed yet.";
            }
            else if (db.TrackingSystemRecords.Where(s => s.WaybillId == id).Count() != 0)
            {
                ViewBag.CancelShipmentError = "Shipment already picked up.";
            }
            if (ViewBag.CancelShipmentError == null)
            {
                shipment.Status = "Cancelled";
                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                Session["HandlingWaybillId"] = id;
                ViewBag.DeliveryAddressEntity = GetAddressEntity(shipment.DeliveryAddress);
                ViewBag.PickupAddressEntity = GetAddressEntity(shipment.PickupAddress);
                if (isPersonalShippingAccount(shipment.SenderId))
                {
                    ViewBag.SenderName = ((PersonalShippingAccount)shipment.Sender).FirstName + " " + ((PersonalShippingAccount)shipment.Sender).LastName;
                }
                else
                {
                    ViewBag.SenderName = ((BusinessShippingAccount)shipment.Sender).ContactPersonName;
                }
                return View(shipment);
            }
        }

        public ActionResult ConfirmShipment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            Session["HandlingWaybillId"] = id;
            ViewBag.DeliveryAddressEntity = GetAddressEntity(shipment.DeliveryAddress);
            ViewBag.PickupAddressEntity = GetAddressEntity(shipment.PickupAddress);
            if (isPersonalShippingAccount(shipment.SenderId))
            {
                ViewBag.SenderName = ((PersonalShippingAccount)shipment.Sender).FirstName + " " + ((PersonalShippingAccount)shipment.Sender).LastName;
            }
            else
            {
                ViewBag.SenderName = ((BusinessShippingAccount)shipment.Sender).ContactPersonName;
            }
            return View(shipment);
        }



        [HttpPost, ActionName("ConfirmShipment")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmingShipment(int? id, bool? IsImmediatePickup, DateTime PickupTime)
        {
            if (id == null || IsImmediatePickup == null || PickupTime == null)
            {
                return RedirectToAction("ConfirmShipment");
            }
            List<string> errorList = new List<string>();
            if (!(bool)IsImmediatePickup)
            {
                if (PickupTime < DateTime.Now)
                {
                    errorList.Add("Pickup can only be arranged in the future.");
                }
                else if (PickupTime.Subtract(DateTime.Now).Days > 4)
                {
                    errorList.Add("Pickup can only be arranged within 5 days.");
                }
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment.NumberOfPackages == 0)
            {
                errorList.Add("You don't have any package in this shipment.");
            }
            else if (shipment.NumberOfPackages > 10)
            {
                errorList.Add("You can have at most 10 packages.");
            }
            if (shipment.RecipientPaysShipment || shipment.RecipientPaysTaxesDuties)
            {
                ShippingAccount Recipient = db.ShippingAccounts.Find(shipment.RecipientId);
                if (Recipient == null)
                {
                    errorList.Add("No valid recipient shipping account found.");
                }
            }
            if (errorList.Count == 0)
            {
                shipment.Status = "Confirmed";
                shipment.isConfirmed = true;
                shipment.IsImmediatePickup = (bool)IsImmediatePickup;
                shipment.PickupTime = PickupTime;

                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                Session["HandlingWaybillId"] = id;
                ViewBag.DeliveryAddressEntity = GetAddressEntity(shipment.DeliveryAddress);
                ViewBag.PickupAddressEntity = GetAddressEntity(shipment.PickupAddress);
                if (isPersonalShippingAccount(shipment.SenderId))
                {
                    ViewBag.SenderName = ((PersonalShippingAccount)shipment.Sender).FirstName + " " + ((PersonalShippingAccount)shipment.Sender).LastName;
                }
                else
                {
                    ViewBag.SenderName = ((BusinessShippingAccount)shipment.Sender).ContactPersonName;
                }
                ViewBag.confirmErrorList = errorList;
                return View(shipment);
            }
        }

        // GET: Shipments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            Session["HandlingWaybillId"] = id;
            ViewBag.DeliveryAddressEntity = GetAddressEntity(shipment.DeliveryAddress);
            ViewBag.PickupAddressEntity = GetAddressEntity(shipment.PickupAddress);
            if (isPersonalShippingAccount(shipment.SenderId))
            {
                ViewBag.SenderName = ((PersonalShippingAccount)shipment.Sender).FirstName + " " + ((PersonalShippingAccount)shipment.Sender).LastName;
            }
            else
            {
                ViewBag.SenderName = ((BusinessShippingAccount)shipment.Sender).ContactPersonName;
            }
            return View(shipment);
        }

        public ActionResult Track(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            Session["HandlingWaybillId"] = id;
            var TrackingSystemRecordList = db.TrackingSystemRecords.Where(s => s.WaybillId == id).OrderByDescending(s => s.DateTimeOfRecord).ToList();
            if (TrackingSystemRecordList.Count == 0)
            {
                TrackingSystemRecordList.Add(new TrackingSystemRecord { TrackingSystemRecordId = -1, DateTimeOfRecord = DateTime.Now, DeliveredTo = "", DeliveredAt = "", Activity = "", Location = "", Remarks = "", Status = "Not Picked Up", WaybillId = 0});
            }
            ViewBag.TrackingSystemRecordList = TrackingSystemRecordList;
            return View(shipment);
        }

        // GET: Shipments/Create
        public ActionResult Create()
        {
            GenerateProvinceCodeList();
            ViewBag.ServiceTypeList = ServiceType.getSelectList();
            GenerateAddressList();
            return View();
        }

        // POST: Shipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "ReferenceNumber,RecipientName,CompanyName,DepartmentName,DeliveryAddress,PhoneNumber,EmailAddress,ServiceType,RecipientPaysShipment,RecipientPaysTaxesDuties,RecipientId,PickupAddress,Origin,Destination,DeliveryEmailNotification,PickupEmailNotification")] Shipment shipment
            )
        {
            shipment.NumberOfPackages = 0;
            shipment.TotalDuties = -1;
            shipment.TotalTaxes = -1;
            shipment.PickupTime = new DateTime(1990, 1, 1);
            shipment.ShippedDate = new DateTime(1990, 1, 1);
            shipment.DeliveredDate = new DateTime(1990, 1, 1);
            shipment.Status = "Created";
            shipment.SenderId = GetCurrentShippingAccountId();
            shipment.isConfirmed = false;
            if (ModelState.IsValid)
            {
                db.Shipments.Add(shipment);
                db.SaveChanges();
                Session["HandlingWaybillId"] = shipment.WaybillId;
                return RedirectToAction("Index", "Packages");
            }
            GenerateProvinceCodeList();
            ViewBag.ServiceTypeList = ServiceType.getSelectList();
            GenerateAddressList();
            return View(shipment);
        }

        // GET: Shipments/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            Session["HandlingWaybillId"] = id;
            if (shipment == null)
            {
                return HttpNotFound();
            }
            GenerateProvinceCodeList();
            GenerateAddressList();
            return View(shipment);
        }

        void ResolveShipmentDifference(Shipment newValues, Shipment oldValues)
        {
            oldValues.ReferenceNumber = newValues.ReferenceNumber;
            oldValues.RecipientName = newValues.RecipientName;
            oldValues.CompanyName = newValues.CompanyName;
            oldValues.DepartmentName = newValues.DepartmentName;
            oldValues.DeliveryAddress = newValues.DeliveryAddress;
            oldValues.PhoneNumber = newValues.PhoneNumber;
            oldValues.EmailAddress = newValues.EmailAddress;
            oldValues.RecipientId = newValues.RecipientId;
            oldValues.ServiceType = newValues.ServiceType;
            oldValues.PickupAddress = newValues.PickupAddress;
            oldValues.Origin = newValues.Origin;
            oldValues.Destination = newValues.Destination;
            oldValues.DeliveryEmailNotification = newValues.DeliveryEmailNotification;
            oldValues.PickupEmailNotification = newValues.PickupEmailNotification;
        }

        // POST: Shipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "ReferenceNumber,RecipientName,CompanyName,DepartmentName,DeliveryAddress,PhoneNumber,EmailAddress,ServiceType,RecipientPaysShipment,RecipientPaysTaxesDuties,RecipientId,PickupAddress,Origin,Destination,DeliveryEmailNotification,PickupEmailNotification")] Shipment shipment
            )
        {
            shipment.WaybillId = (int)Session["HandlingWaybillId"];
            Shipment oldShipment = db.Shipments.Find(shipment.WaybillId);
            ResolveShipmentDifference(shipment, oldShipment);

            if (ModelState.IsValid)
            {
                db.Entry(oldShipment).State = EntityState.Modified;
                db.SaveChanges();

                Session["HandlingWaybillId"] = shipment.WaybillId;
                return RedirectToAction("Index", "Packages");
            }
            GenerateProvinceCodeList();
            GenerateAddressList();
            return View(shipment);
        }

        public ActionResult EnterFeeAndWeight()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnterFeeAndWeight(int? WaybillId)

        {
            if (WaybillId != null)
            {
                return RedirectToAction("EmployeeEdit", new { id = WaybillId });
            }
            return View();
        }


        public ActionResult EmployeeEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            Session["HandlingWaybillId"] = id;
            if (shipment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackageList = shipment.Packages;
            return View(shipment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeEdit(
            decimal? Package0, int? Package0id,
            decimal? Package1, int? Package1id,
            decimal? Package2, int? Package2id,
            decimal? Package3, int? Package3id,
            decimal? Package4, int? Package4id,
            decimal? Package5, int? Package5id,
            decimal? Package6, int? Package6id,
            decimal? Package7, int? Package7id,
            decimal? Package8, int? Package8id,
            decimal? Package9, int? Package9id,
            decimal? TotalTaxes,
            decimal? TotalDuties
            )
        {
            if (Session["HandlingWaybillId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int WaybillId = (int)Session["HandlingWaybillId"];
            Shipment shipment = db.Shipments.Find(WaybillId);
            if (Package0id != null && Package0 != null) { Package p = db.Packages.Find((int)Package0id); p.Weight = (decimal)Package0; db.Entry(p).State = EntityState.Modified; }
            if (Package1id != null && Package1 != null) { Package p = db.Packages.Find((int)Package1id); p.Weight = (decimal)Package1; db.Entry(p).State = EntityState.Modified; }
            if (Package2id != null && Package2 != null) { Package p = db.Packages.Find((int)Package2id); p.Weight = (decimal)Package2; db.Entry(p).State = EntityState.Modified; }
            if (Package3id != null && Package3 != null) { Package p = db.Packages.Find((int)Package3id); p.Weight = (decimal)Package3; db.Entry(p).State = EntityState.Modified; }
            if (Package4id != null && Package4 != null) { Package p = db.Packages.Find((int)Package4id); p.Weight = (decimal)Package4; db.Entry(p).State = EntityState.Modified; }
            if (Package5id != null && Package5 != null) { Package p = db.Packages.Find((int)Package5id); p.Weight = (decimal)Package5; db.Entry(p).State = EntityState.Modified; }
            if (Package6id != null && Package6 != null) { Package p = db.Packages.Find((int)Package6id); p.Weight = (decimal)Package6; db.Entry(p).State = EntityState.Modified; }
            if (Package7id != null && Package7 != null) { Package p = db.Packages.Find((int)Package7id); p.Weight = (decimal)Package7; db.Entry(p).State = EntityState.Modified; }
            if (Package8id != null && Package8 != null) { Package p = db.Packages.Find((int)Package8id); p.Weight = (decimal)Package8; db.Entry(p).State = EntityState.Modified; }
            if (Package9id != null && Package9 != null) { Package p = db.Packages.Find((int)Package9id); p.Weight = (decimal)Package9; db.Entry(p).State = EntityState.Modified; }
            if (TotalTaxes != null)
            {
                shipment.TotalTaxes = (decimal)TotalTaxes;
                db.Entry(shipment).State = EntityState.Modified;
            }
            if (TotalDuties != null)
            {
                shipment.TotalDuties = (decimal)TotalDuties;
                db.Entry(shipment).State = EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("Index");


            ViewBag.PackageList = shipment.Packages;
            return View(shipment);
        }

        // GET: Shipments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            Session["HandlingWaybillId"] = id;
            ViewBag.DeliveryAddressEntity = GetAddressEntity(shipment.DeliveryAddress);
            ViewBag.PickupAddressEntity = GetAddressEntity(shipment.PickupAddress);
            if (isPersonalShippingAccount(shipment.SenderId))
            {
                ViewBag.SenderName = ((PersonalShippingAccount)shipment.Sender).FirstName + " " + ((PersonalShippingAccount)shipment.Sender).LastName;
            }
            else
            {
                ViewBag.SenderName = ((BusinessShippingAccount)shipment.Sender).ContactPersonName;
            }
            return View(shipment);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shipment shipment = db.Shipments.Find(id);
            db.Shipments.Remove(shipment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Shipments/GenerateHistoryReport
        [Authorize(Roles = "Employee,Customer")]
        public ActionResult GenerateHistoryReport(int? ShippingAccountId, DateTime? DateFrom, DateTime? DateTo, string sortOrder, int? currentShippingAccountId, int? page)
        {
            // Instantiate an instance of the ShipmentsReportViewModel and the ShipmentsSearchViewModel.
            var shipmentSearch = new ShipmentsReportViewModel();
            shipmentSearch.Shipment = new ShipmentsSearchViewModel();

            // Code for paging.
            ViewBag.CurrentSort = sortOrder;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

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
                else
                {
                    page = 1;
                }
            }

            ViewBag.CurrentShippingAccountId = ShippingAccountId;

            // Populate the ShippingAccountId dropdown list.
            shipmentSearch.Shipment.ShippingAccounts = PopulateShippingAccountsDropdownList().ToList();

            // Initialize the query to retrieve shipments using the ShipmentsListViewModel.
            var shipmentQuery = from s in db.Shipments
                                select new ShipmentsListViewModel
                                {
                                    WaybillId = s.WaybillId,
                                    ServiceType = s.ServiceType,
                                    ShippedDate = s.ShippedDate,
                                    DeliveredDate = s.DeliveredDate,
                                    RecipientName = s.RecipientName,
                                    NumberOfPackages = s.NumberOfPackages,
                                    Origin = s.Origin,
                                    Destination = s.Destination,
                                    ShippingAccountId = s.SenderId
                                };

            // Add the condition to select a spefic shipping account if shipping account id is not null.
            if (ShippingAccountId != null)
            {
                // TODO: Construct the LINQ query to retrive only the shipments for the specified shipping account id.
                shipmentQuery = shipmentQuery.Where(s => s.ShippingAccountId == ShippingAccountId);

                // Code for date range search
                if (DateFrom != null)
                {
                    shipmentQuery = shipmentQuery.Where(s => s.ShippedDate >= DateFrom).Where(s => s.DeliveredDate >= DateFrom);
                }
                if (DateTo != null)
                {
                    shipmentQuery = shipmentQuery.Where(s => s.ShippedDate <= DateTo).Where(s => s.DeliveredDate <= DateTo);
                }

                // Code for sorting.
                ViewBag.WaybillIdSortParm = string.IsNullOrEmpty(sortOrder) ? "waybillId" : "";
                ViewBag.ServiceTypeSortParm = sortOrder == "serviceType" ? "serviceType_desc" : "serviceType";
                ViewBag.ShippedDateSortParm = sortOrder == "shippedDate" ? "shippedDate_desc" : "shippedDate";
                ViewBag.DeliveredDateSortParm = sortOrder == "deliveredDate" ? "deliveredDate_desc" : "deliveredDate";
                ViewBag.RecipientNameSortParm = sortOrder == "recipientName" ? "recipientName_desc" : "recipientName";
                ViewBag.OriginSortParm = sortOrder == "origin" ? "origin_desc" : "origin";
                ViewBag.DestinationSortParm = sortOrder == "destination" ? "destination_desc" : "destination";
                switch (sortOrder)
                {
                    case "waybillId":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.WaybillId);
                        break;
                    case "serviceType":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.ServiceType);
                        break;
                    case "serviceType_desc":
                        shipmentQuery = shipmentQuery.OrderByDescending(s => s.ServiceType);
                        break;
                    case "shippedDate":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.ShippedDate);
                        break;
                    case "shippedDate_desc":
                        shipmentQuery = shipmentQuery.OrderByDescending(s => s.ShippedDate);
                        break;
                    case "deliveredDate":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.DeliveredDate);
                        break;
                    case "deliveredDate_desc":
                        shipmentQuery = shipmentQuery.OrderByDescending(s => s.DeliveredDate);
                        break;
                    case "recipientName":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.RecipientName);
                        break;
                    case "recipientName_desc":
                        shipmentQuery = shipmentQuery.OrderByDescending(s => s.RecipientName);
                        break;
                    case "origin":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.Origin);
                        break;
                    case "origin_desc":
                        shipmentQuery = shipmentQuery.OrderByDescending(s => s.Origin);
                        break;
                    case "destination":
                        shipmentQuery = shipmentQuery.OrderBy(s => s.Destination);
                        break;
                    case "destination_desc":
                        shipmentQuery = shipmentQuery.OrderByDescending(s => s.Destination);
                        break;
                    default:
                        shipmentQuery = shipmentQuery.OrderBy(s => s.WaybillId);
                        break;
                }

                shipmentSearch.Shipments = shipmentQuery.ToPagedList(pageNumber, pageSize);
            }
            else
            {
                // Return an empty result if no shipping account id has been selected.
                shipmentSearch.Shipments = new ShipmentsListViewModel[0].ToPagedList(pageNumber, pageSize);
            }

            return View(shipmentSearch);
        }

        private SelectList PopulateShippingAccountsDropdownList()
        {
            // TODO: Construct the LINQ query to retrieve the unique list of shipping account ids.
            var shippingAccountQuery = db.Shipments.Select(s => s.SenderId).Distinct().OrderBy(s => s);
            return new SelectList(shippingAccountQuery);
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
