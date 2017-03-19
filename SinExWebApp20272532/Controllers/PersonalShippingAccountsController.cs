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
    public class PersonalShippingAccountsController : Controller
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();

        /*
        // GET: PersonalShippingAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonalShippingAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShippingAccountId,Building,Street,City,ProvinceCode,PostalCode,CreditCardType,CreditCardNumber,CreditCardSecurityNumber,CreditCardCardholderName,CreditCardExpiryMonth,CreditCardExpiryYear,PhoneNumber,EmailAddress,FirstName,LastName")] PersonalShippingAccount personalShippingAccount)
        {
            if (ModelState.IsValid)
            {
                //db.ShippingAccounts.Add(personalShippingAccount);
                //db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(personalShippingAccount);
        }
        */

        // GET: PersonalShippingAccounts/Edit/5
        [Authorize(Roles = "Customer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalShippingAccount personalShippingAccount = (PersonalShippingAccount)db.ShippingAccounts.Find(id);
            if (personalShippingAccount == null || personalShippingAccount.UserName != User.Identity.Name)
            {
                return HttpNotFound();
            }
            return View(personalShippingAccount);
        }

        // POST: PersonalShippingAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShippingAccountId,Building,Street,City,ProvinceCode,PostalCode,CreditCardType,CreditCardNumber,CreditCardSecurityNumber,CreditCardCardholderName,CreditCardExpiryMonth,CreditCardExpiryYear,PhoneNumber,EmailAddress,FirstName,LastName")] PersonalShippingAccount personalShippingAccount)
        {
            if (personalShippingAccount.UserName != User.Identity.Name)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.Entry(personalShippingAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(personalShippingAccount);
        }

        // GET: BusinessShippingAccount/GetBusinessShippingAccountRecord
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult GetPersonalShippingAccountRecord()
        {
            string userName = System.Web.HttpContext.Current.User.Identity.Name;
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalShippingAccount personalShippingAccount = (PersonalShippingAccount)db.ShippingAccounts.SingleOrDefault(s => s.UserName == userName);
            if (personalShippingAccount == null)
            {
                return HttpNotFound("There is no personal shipping account with user name \"" + userName + "\"");
            }
            return View(personalShippingAccount);
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
