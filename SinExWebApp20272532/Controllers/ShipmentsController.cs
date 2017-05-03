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

namespace SinExWebApp20272532.Controllers
{
    public class ShipmentsController : Controller
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
            ViewBag.AddressList = Address.GetSelectList(ShippingAccountId);
        }

        // GET: Shipments
        public ActionResult Index()
        {
            return View(db.Shipments.Where(s => s.Status != "Confirmed").ToList());
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
            return View(shipment);
        }

        [HttpPost,ActionName("ConfirmShipment")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmingShipment(int? id)
        {
            Shipment shipment = db.Shipments.Find(id);
            shipment.Status = "Confirmed";
            db.Entry(shipment).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
            return View(shipment);
        }

        // GET: Shipments/Create
        public ActionResult Create()
        {
            GenerateProvinceCodeList();
            GenerateAddressList();
            return View();
        }

        // POST: Shipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "ReferenceNumber,RecipientName,CompanyName,DepartmentName,PhoneNumber,EmailAddress,RecipientId,ServiceType,ShipmentPayerId,TaxesDutiesPayerId,NumberOfPackages,Origin,Destination,DeliveryEmailNotification,PickupEmailNotification")] Shipment shipment
            )
        {
            if (ModelState.IsValid)
            {
                shipment.TotalDuties = -1;
                shipment.TotalTaxes = -1;
                shipment.ShippedDate = new DateTime(1990,1,1);
                shipment.DeliveredDate = new DateTime(1990, 1, 1);
                shipment.Status = "Created";
                shipment.SenderId = db.ShippingAccounts.Where(s => s.UserName == User.Identity.Name).Select(s => s.ShippingAccountId).Single();

                db.Shipments.Add(shipment);
                db.SaveChanges();
                Session["HandlingWaybillId"] = shipment.WaybillId;
                return RedirectToAction("Index","Packages",new { waybillId = shipment.WaybillId});
            }
            GenerateProvinceCodeList();
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
            if (shipment == null)
            {
                return HttpNotFound();
            }
            GenerateProvinceCodeList();
            GenerateAddressList();
            Session["HandlingWaybillId"] = shipment.WaybillId;
            return View(shipment);
        }

        // POST: Shipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "ReferenceNumber,RecipientName,CompanyName,DepartmentName,PhoneNumber,EmailAddress,RecipientId,ServiceType,ShipmentPayerId,TaxesDutiesPayerId,NumberOfPackages,Origin,Destination,DeliveryEmailNotification,PickupEmailNotification")] Shipment shipment
            )
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            GenerateProvinceCodeList();
            GenerateAddressList();
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
