using SinExWebApp20272532.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SinExWebApp20272532.Controllers
{
    public class BaseController : Controller
    {
        private SinExDatabaseContext db = new SinExDatabaseContext();
        public decimal CurrencyExchange(decimal sourceAmount, decimal sourceRate, decimal exchangeRate)
        {
            decimal target = 0;

            target = sourceAmount * exchangeRate / sourceRate;

            return target;
        }

        public int GetCurrentShippingAccountId()
        {
            var db = new Models.SinExDatabaseContext();
            return db.ShippingAccounts.Where(s => s.UserName == User.Identity.Name).Select(s => s.ShippingAccountId).Single();
        }

        public void UpdateShipmentFee(Shipment shipment, bool estimated)
        {
            /*
            decimal fee = 0;
            foreach (var p in shipment.Packages)
            {
                if (estimated)
                {
                    ServiceType st = db.ServiceTypes.Where(s => s.Type == shipment.ServiceType).Single();
                    PackageType pt = p.PackageTypeSize.PackageType;
                    ServicePackageFee spf = db.ServicePackageFees.Where(s => s.ServiceTypeID == st.ServiceTypeID).Where(s => s.PackageTypeID == pt.PackageTypeID).Single();
                    decimal PackageFee, FeePerKG, MinimumFee, MaximumWeight;
                    FeePerKG = spf.Fee;
                    try
                    {
                        MaximumWeight = p.PackageTypeSize.WeightLimit;
                    }
                    catch (Exception)
                    {
                        MaximumWeight = 0;
                    }

                    MinimumFee = spf.MinimumFee;
                    PackageFee = (decimal)p.EstimatedWeight * FeePerKG;
                    if (PackageFee < MinimumFee)
                    {
                        PackageFee = MinimumFee;
                    }
                    if (p.EstimatedWeight > MaximumWeight && MaximumWeight != 0)
                    {
                        PackageFee += spf.Penalty;
                    }
                    if (spf.PackageTypeID == 1)
                    {
                        PackageFee = MinimumFee;
                    }
                    fee += PackageFee;
                }
                else
                {
                    ServiceType st = db.ServiceTypes.Where(s => s.Type == shipment.ServiceType).Single();
                    PackageType pt = p.PackageTypeSize.PackageType;
                    ServicePackageFee spf = db.ServicePackageFees.Where(s => s.ServiceTypeID == st.ServiceTypeID).Where(s => s.PackageTypeID == pt.PackageTypeID).Single();
                    decimal PackageFee, FeePerKG, MinimumFee, MaximumWeight;
                    FeePerKG = spf.Fee;
                    try
                    {
                        MaximumWeight = p.PackageTypeSize.WeightLimit;
                    }
                    catch (Exception)
                    {
                        MaximumWeight = 0;
                    }

                    MinimumFee = spf.MinimumFee;
                    PackageFee = (decimal)p.Weight * FeePerKG;
                    if (PackageFee < MinimumFee)
                    {
                        PackageFee = MinimumFee;
                    }
                    if (p.Weight > MaximumWeight && MaximumWeight != 0)
                    {
                        PackageFee += spf.Penalty;
                    }
                    if (spf.PackageTypeID == 1)
                    {
                        PackageFee = MinimumFee;
                    }
                    fee += PackageFee;
                }

            }
            Shipment ss = db.Shipments.Find(shipment.WaybillId);
            ss.ShipmentFee = fee;
            db.Entry(ss).State = EntityState.Modified;
            db.SaveChanges();
            */
        }

        public void ComposeInvoice(Shipment shipment)
        {
            if (shipment.RecipientPaysShipment == shipment.RecipientPaysTaxesDuties)
            {
                Invoice invoice = new Invoice();
                invoice.WaybillId = shipment.WaybillId;
                invoice.ShipDate = db.TrackingSystemRecords.Where(s => s.WaybillId == shipment.WaybillId).OrderBy(S => S.DateTimeOfRecord).First().DateTimeOfRecord;
                ShippingAccount rec;
                if (shipment.RecipientPaysShipment)
                {
                    rec =  db.ShippingAccounts.Find(shipment.RecipientId);
                }
                else
                {
                    rec = shipment.Sender;
                }
                invoice.ShippingAccountId = rec.ShippingAccountId;
                try
                {
                    BusinessShippingAccount BSA = (BusinessShippingAccount)rec;
                    invoice.RecipientName = BSA.ContactPersonName;
                }
                catch (Exception)
                {
                    PersonalShippingAccount PSA = (PersonalShippingAccount)rec;
                    invoice.RecipientName = PSA.FirstName + " " + PSA.LastName;
                }
                invoice.Origin = shipment.Origin;
                invoice.Destination = shipment.Destination;
                invoice.ServiceType = shipment.ServiceType;
                invoice.InvoiceAmount = shipment.ShipmentFee + shipment.TotalTaxes + shipment.TotalDuties;
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                Invoice invoice = new Invoice();
                Invoice invoice2 = new Invoice();
                invoice.WaybillId = shipment.WaybillId;
                invoice.ShipDate = db.TrackingSystemRecords.Where(s => s.WaybillId == shipment.WaybillId).OrderBy(S => S.DateTimeOfRecord).First().DateTimeOfRecord;
                invoice2.WaybillId = shipment.WaybillId;
                invoice2.ShipDate = db.TrackingSystemRecords.Where(s => s.WaybillId == shipment.WaybillId).OrderBy(S => S.DateTimeOfRecord).First().DateTimeOfRecord;
                ShippingAccount sen, rec;
                sen = shipment.Sender;
                rec = db.ShippingAccounts.Find(shipment.RecipientId);
                try
                {
                    BusinessShippingAccount BSA = (BusinessShippingAccount)sen;
                    invoice.RecipientName = BSA.ContactPersonName;
                }
                catch (Exception)
                {
                    PersonalShippingAccount PSA = (PersonalShippingAccount)sen;
                    invoice.RecipientName = PSA.FirstName + " " + PSA.LastName;
                }
                try
                {
                    BusinessShippingAccount BSA = (BusinessShippingAccount)rec;
                    invoice2.RecipientName = BSA.ContactPersonName;
                }
                catch (Exception)
                {
                    PersonalShippingAccount PSA = (PersonalShippingAccount)rec;
                    invoice2.RecipientName = PSA.FirstName + " " + PSA.LastName;
                }
                invoice.Origin = shipment.Origin;
                invoice.Destination = shipment.Destination;
                invoice.ServiceType = shipment.ServiceType;
                invoice2.Origin = shipment.Origin;
                invoice2.Destination = shipment.Destination;
                invoice2.ServiceType = shipment.ServiceType;
                if (shipment.RecipientPaysShipment)
                {
                    invoice.InvoiceAmount = shipment.ShipmentFee;
                    invoice2.InvoiceAmount =  shipment.TotalTaxes + shipment.TotalDuties;
                }
                else
                {
                    invoice2.InvoiceAmount = shipment.ShipmentFee;
                    invoice.InvoiceAmount = shipment.TotalTaxes + shipment.TotalDuties;
                }
                db.Entry(invoice).State = EntityState.Modified;
                db.Entry(invoice2).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}