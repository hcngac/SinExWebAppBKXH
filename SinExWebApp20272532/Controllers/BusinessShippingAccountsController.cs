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
    public class BusinessShippingAccountsController : Controller
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        /*
        // GET: BusinessShippingAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessShippingAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShippingAccountId,Building,Street,City,ProvinceCode,PostalCode,CreditCardType,CreditCardNumber,CreditCardSecurityNumber,CreditCardCardholderName,CreditCardExpiryMonth,CreditCardExpiryYear,PhoneNumber,EmailAddress,ContactPersonName,CompanyName,DepartmentName")] BusinessShippingAccount businessShippingAccount)
        {
            if (ModelState.IsValid)
            {
                //db.ShippingAccounts.Add(businessShippingAccount);
                //db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(businessShippingAccount);
        }
        */

        // GET: BusinessShippingAccounts/Edit/5
        [Authorize(Roles ="Customer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessShippingAccount businessShippingAccount = (BusinessShippingAccount)db.ShippingAccounts.Find(id);
            if (businessShippingAccount == null || businessShippingAccount.UserName != User.Identity.Name)
            {
                return HttpNotFound();
            }

            SelectList creditCardTypes = new SelectList(new string[] {
                "Please Select"     ,
                "American Express"  ,
                "Diners Club"       ,
                "Discover"          ,
                "MasterCard"        ,
                "UnionPay"          ,
                "Visa"
            });
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

            ViewBag.CreditCardTypes = creditCardTypes;
            ViewBag.ProvinceCodeList = proviceCode;

            return View(businessShippingAccount);
        }

        // POST: BusinessShippingAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,ShippingAccountId,Building,Street,City,ProvinceCode,PostalCode,CreditCardType,CreditCardNumber,CreditCardSecurityNumber,CreditCardCardholderName,CreditCardExpiryMonth,CreditCardExpiryYear,PhoneNumber,Email,ContactPersonName,CompanyName,DepartmentName")] BusinessShippingAccount businessShippingAccount)
        {
            if (businessShippingAccount.UserName != User.Identity.Name)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.Entry(businessShippingAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(businessShippingAccount);
        }

        // GET: BusinessShippingAccount/GetBusinessShippingAccountRecord
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult GetBusinessShippingAccountRecord()
        {
            string userName = System.Web.HttpContext.Current.User.Identity.Name;
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessShippingAccount businessShippingAccount = (BusinessShippingAccount)db.ShippingAccounts.SingleOrDefault(s => s.UserName == userName);
            if (businessShippingAccount == null)
            {
                return HttpNotFound("There is no business shipping account with user name \"" + userName + "\"");
            }
            return View(businessShippingAccount);
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
