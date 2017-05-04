﻿using System;
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
            return View(db.Shipments.Where(s => s.isConfirmed == false).ToList());
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
        public ActionResult EnterFeeAndWeight([Bind(Include = "ReferenceNumber,RecipientName,CompanyName,DepartmentName,PhoneNumber,EmailAddress,RecipientId,ServiceType,ShipmentPayerId,TaxesDutiesPayerId,NumberOfPackages,Origin,Destination,DeliveryEmailNotification,PickupEmailNotification")] Shipment shipment
            )

        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(shipment);
        }


        public ActionResult EmployeeEdit(string id)
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
            return View(shipment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeEdit([Bind(Include = "ReferenceNumber,RecipientName,CompanyName,DepartmentName,PhoneNumber,EmailAddress,RecipientId,ServiceType,ShipmentPayerId,TaxesDutiesPayerId,NumberOfPackages,Origin,Destination,DeliveryEmailNotification,PickupEmailNotification")] Shipment shipment
            )

        {
            if (ModelState.IsValid)
            {
                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
