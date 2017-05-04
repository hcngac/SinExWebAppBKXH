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
            decimal fee = 0;
            foreach (var p in shipment.Packages)
            {
                if (estimated)
                {
                    ServicePackageFee spf = db.ServicePackageFees.Where(s => s.ServiceType.Type == shipment.ServiceType).Where(s => s.PackageType == p.PackageTypeSize.PackageType).Single();
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
                    ServicePackageFee spf = db.ServicePackageFees.Where(s => s.ServiceType.Type == shipment.ServiceType).Where(s => s.PackageType == p.PackageTypeSize.PackageType).Single();
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
            shipment.ShipmentFee = fee;
            db.Entry(shipment).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void ComposeInvoice(Shipment shipment)
        {
            Invoice invoice = new Invoice();
            invoice.WaybillId = shipment.WaybillId;
            invoice.ShipDate = db.TrackingSystemRecords.Where(s => s.WaybillId == shipment.WaybillId).OrderBy(S => S.DateTimeOfRecord).First().DateTimeOfRecord;
        }
    }
}