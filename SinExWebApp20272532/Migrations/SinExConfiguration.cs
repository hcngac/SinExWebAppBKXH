namespace SinExWebApp20272532.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using System.Collections.Generic;

    internal sealed class SinExConfiguration : DbMigrationsConfiguration<SinExWebApp20272532.Models.SinExDatabaseContext>
    {
        public SinExConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "SinExWebApp20272532.Models.SinExDatabaseContext";
        }

        protected override void Seed(SinExWebApp20272532.Models.SinExDatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            // Add package type data.
            context.PackageTypes.AddOrUpdate(
                p => p.PackageTypeID,
                new PackageType
                {
                    PackageTypeID = 1,
                    Type = "Envelope",
                    Description = "for correspondence and documents only with no commercial value"
                },
                new PackageType
                {
                    PackageTypeID = 2,
                    Type = "Pak",
                    Description = "for flat, non-breakable articles including heavy documents"
                },
                new PackageType
                {
                    PackageTypeID = 3,
                    Type = "Tube",
                    Description = "for larger documents, such as blueprints, which should be rolled rather than folded"
                },
                new PackageType
                {
                    PackageTypeID = 4,
                    Type = "Box",
                    Description = "for bulky items, such as electronic parts and textile samples"
                },
                new PackageType
                {
                    PackageTypeID = 5,
                    Type = "Customer",
                    Description = "for packaging provided by customer"
                }
                );

            // Add service type data.
            context.ServiceTypes.AddOrUpdate(
                p => p.ServiceTypeID,
                new ServiceType { ServiceTypeID = 1, Type = "Same Day", CutoffTime = "10:00 a.m.", DeliveryTime = "Same day" },
                new ServiceType { ServiceTypeID = 2, Type = "Next Day 10:30", CutoffTime = "3:00 p.m.", DeliveryTime = "Next day 10:30 a.m." },
                new ServiceType { ServiceTypeID = 3, Type = "Next Day 12:00", CutoffTime = "6:00 p.m.", DeliveryTime = "Next day 12:00 p.m." },
                new ServiceType { ServiceTypeID = 4, Type = "Next Day 15:00", CutoffTime = "9:00 p.m.", DeliveryTime = "Next day 15:00 p.m." },
                new ServiceType { ServiceTypeID = 5, Type = "2nd Day", CutoffTime = "", DeliveryTime = "5:00 p.m. second business day" },
                new ServiceType { ServiceTypeID = 6, Type = "Ground", CutoffTime = "", DeliveryTime = "1 to 5 business days" }
                );

            // Add service and package type fees.
            context.ServicePackageFees.AddOrUpdate(
                p => p.ServicePackageFeeID,
                // Same Day
                new ServicePackageFee { ServicePackageFeeID = 1, Fee = 160, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 1, Penalty = 500 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 2, Fee = 100, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 2, Penalty = 500 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 3, Fee = 100, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 3, Penalty = 500 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 4, Fee = 110, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 4, Penalty = 500 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 5, Fee = 110, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 5, Penalty = 500 }, // Customer
                                                                                                                                      // Next Day 10:30
                new ServicePackageFee { ServicePackageFeeID = 6, Fee = 140, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 1, Penalty = 500 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 7, Fee = 90, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 2, Penalty = 500 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 8, Fee = 90, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 3, Penalty = 500 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 9, Fee = 99, MinimumFee = 100, ServiceTypeID = 2, PackageTypeID = 4, Penalty = 500 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 10, Fee = 99, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 5, Penalty = 500 }, // Customer
                                                                                                                                      // Next Day 12:00
                new ServicePackageFee { ServicePackageFeeID = 11, Fee = 130, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 1, Penalty = 500 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 12, Fee = 80, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 2, Penalty = 500 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 13, Fee = 80, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 3, Penalty  = 500 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 14, Fee = 88, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 4, Penalty  = 500 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 15, Fee = 88, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 5, Penalty  = 500 }, // Customer
                                                                                                                                      // Next Day 15:00
                new ServicePackageFee { ServicePackageFeeID = 16, Fee = 120, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 1, Penalty = 500 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 17, Fee = 70, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 2, Penalty = 500 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 18, Fee = 70, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 3, Penalty  = 500 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 19, Fee = 77, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 4, Penalty  = 500 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 20, Fee = 77, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 5, Penalty  = 500 }, // Customer
                                                                                                                                      // 2nd Day
                new ServicePackageFee { ServicePackageFeeID = 21, Fee = 50, MinimumFee = 50, ServiceTypeID = 5, PackageTypeID = 1, Penalty = 500 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 22, Fee = 50, MinimumFee = 50, ServiceTypeID = 5, PackageTypeID = 2, Penalty  = 500 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 23, Fee = 50, MinimumFee = 50, ServiceTypeID = 5, PackageTypeID = 3, Penalty  = 500 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 24, Fee = 55, MinimumFee = 55, ServiceTypeID = 5, PackageTypeID = 4, Penalty  = 500 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 25, Fee = 55, MinimumFee = 55, ServiceTypeID = 5, PackageTypeID = 5, Penalty  = 500 }, // Customer
                                                                                                                                     // Ground
                new ServicePackageFee { ServicePackageFeeID = 26, Fee = 25, MinimumFee = 25, ServiceTypeID = 6, PackageTypeID = 1, Penalty = 500 },// Envelope
                new ServicePackageFee { ServicePackageFeeID = 27, Fee = 25, MinimumFee = 25, ServiceTypeID = 6, PackageTypeID = 2, Penalty  = 500 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 28, Fee = 25, MinimumFee = 25, ServiceTypeID = 6, PackageTypeID = 3, Penalty  = 500 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 29, Fee = 30, MinimumFee = 30, ServiceTypeID = 6, PackageTypeID = 4, Penalty  = 500 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 30, Fee = 30, MinimumFee = 30, ServiceTypeID = 6, PackageTypeID = 5, Penalty  = 500 }  // Customer
                );

            context.Currencies.AddOrUpdate(
                p => p.CurrencyCode,
                new Currency { CurrencyCode = "CNY", ExchangeRate = 1.00M },
                new Currency { CurrencyCode = "HKD", ExchangeRate = 1.13M },
                new Currency { CurrencyCode = "TWD", ExchangeRate = 4.56M },
                new Currency { CurrencyCode = "MOP", ExchangeRate = 1.16M }
                );

            context.Destinations.AddOrUpdate(
                p => p.DestinationID,
                new Destination { DestinationID = 1, ProvinceCode = "BJ", City = "Beijing" },
                new Destination { DestinationID = 2, ProvinceCode = "JL", City = "Changchun" },
                new Destination { DestinationID = 3, ProvinceCode = "HN", City = "Changsha" },
                new Destination { DestinationID = 4, ProvinceCode = "SC", City = "Chengdu" },
                new Destination { DestinationID = 5, ProvinceCode = "CQ", City = "Chongqing" },
                new Destination { DestinationID = 6, ProvinceCode = "JX", City = "Fuzhou" },
                new Destination { DestinationID = 7, ProvinceCode = "QH", City = "Golmud" },
                new Destination { DestinationID = 8, ProvinceCode = "GD", City = "Guangzhou" },
                new Destination { DestinationID = 9, ProvinceCode = "GZ", City = "Guiyang" },
                new Destination { DestinationID = 10, ProvinceCode = "HI", City = "Haikou" },
                new Destination { DestinationID = 11, ProvinceCode = "NM", City = "Hailar" },
                new Destination { DestinationID = 12, ProvinceCode = "ZJ", City = "Hangzhou" },
                new Destination { DestinationID = 13, ProvinceCode = "HL", City = "Harbin" },
                new Destination { DestinationID = 14, ProvinceCode = "AH", City = "Hefei" },
                new Destination { DestinationID = 15, ProvinceCode = "NM", City = "Hohhot" },
                new Destination { DestinationID = 16, ProvinceCode = "HK", City = "Hong Kong" },
                new Destination { DestinationID = 17, ProvinceCode = "NM", City = "Hulun Buir" },
                new Destination { DestinationID = 18, ProvinceCode = "SD", City = "Jinan" },
                new Destination { DestinationID = 19, ProvinceCode = "XJ", City = "Kashi" },
                new Destination { DestinationID = 20, ProvinceCode = "YN", City = "Kunming" },
                new Destination { DestinationID = 21, ProvinceCode = "GS", City = "Lanzhou" },
                new Destination { DestinationID = 22, ProvinceCode = "XZ", City = "Lhasa" },
                new Destination { DestinationID = 23, ProvinceCode = "MC", City = "Macau" },
                new Destination { DestinationID = 24, ProvinceCode = "JX", City = "Nanchang" },
                new Destination { DestinationID = 25, ProvinceCode = "JS", City = "Nanjing" },
                new Destination { DestinationID = 26, ProvinceCode = "JX", City = "Nanning" },
                new Destination { DestinationID = 27, ProvinceCode = "HL", City = "Qiqihar" },
                new Destination { DestinationID = 28, ProvinceCode = "SH", City = "Shanghai" },
                new Destination { DestinationID = 29, ProvinceCode = "LN", City = "Shenyang" },
                new Destination { DestinationID = 30, ProvinceCode = "HE", City = "Shijiazhuang" },
                new Destination { DestinationID = 31, ProvinceCode = "TW", City = "Taipei" },
                new Destination { DestinationID = 32, ProvinceCode = "SX", City = "Taiyuan" },
                new Destination { DestinationID = 33, ProvinceCode = "HE", City = "Tianjin" },
                new Destination { DestinationID = 34, ProvinceCode = "XJ", City = "Urumqi" },
                new Destination { DestinationID = 35, ProvinceCode = "HB", City = "Wuhan" },
                new Destination { DestinationID = 36, ProvinceCode = "SN", City = "Xi'an" },
                new Destination { DestinationID = 37, ProvinceCode = "QH", City = "Xining" },
                new Destination { DestinationID = 38, ProvinceCode = "NX", City = "Yinchuan" },
                new Destination { DestinationID = 39, ProvinceCode = "GS", City = "Yumen" },
                new Destination { DestinationID = 40, ProvinceCode = "HA", City = "Zhengzhou" }
                );

            context.PackageTypeSizes.AddOrUpdate(
                p => p.PackageTypeSizeID,
                new PackageTypeSize { PackageTypeSizeID = 1, Size = "250x300mm", WeightLimit = 0, PackageTypeID = 1 },
                new PackageTypeSize { PackageTypeSizeID = 2, Size = "small - 350x400mm", WeightLimit = 5, PackageTypeID = 2 },
                new PackageTypeSize { PackageTypeSizeID = 3, Size = "large - 450x550mm", WeightLimit = 5, PackageTypeID = 2 },
                new PackageTypeSize { PackageTypeSizeID = 4, Size = "1000x80mm", WeightLimit = 0, PackageTypeID = 3 },
                new PackageTypeSize { PackageTypeSizeID = 5, Size = "small - 300x250x150mm", WeightLimit = 10, PackageTypeID = 4 },
                new PackageTypeSize { PackageTypeSizeID = 6, Size = "medium - 400x350x250mm", WeightLimit = 20, PackageTypeID = 4 },
                new PackageTypeSize { PackageTypeSizeID = 7, Size = "large - 500x450x350mm", WeightLimit = 30, PackageTypeID = 4 },
                new PackageTypeSize { PackageTypeSizeID = 8, Size = "no size limit", WeightLimit = 0, PackageTypeID = 5 }
                );
            
            /*
            // Add shipment data.
            context.Shipments.AddOrUpdate(
                p => p.WaybillId,
                new Shipment { WaybillId = 1, ReferenceNumber = "", ServiceType = "Same Day", ShippedDate = new DateTime(2016, 11, 11), DeliveredDate = new DateTime(2016, 11, 11), RecipientName = "Andy Ho", NumberOfPackages = 1, Origin = "Hong Kong", Destination = "Guangzhou", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 2, ReferenceNumber = "A28756", ServiceType = "Same Day", ShippedDate = new DateTime(2016, 12, 12), DeliveredDate = new DateTime(2016, 12, 12), RecipientName = "John Wong", NumberOfPackages = 2, Origin = "Hong Kong", Destination = "Macau", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 3, ReferenceNumber = "", ServiceType = "Next Day 10:30", ShippedDate = new DateTime(2016, 12, 31), DeliveredDate = new DateTime(2017, 01, 01), RecipientName = "John Wong", NumberOfPackages = 1, Origin = "Hong Kong", Destination = "Beijing", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 4, ReferenceNumber = "", ServiceType = "Next Day 10:30", ShippedDate = new DateTime(2016, 11, 30), DeliveredDate = new DateTime(2016, 12, 01), RecipientName = "Daisy Chan", NumberOfPackages = 3, Origin = "Hong Kong", Destination = "Shanghai", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 5, ReferenceNumber = "", ServiceType = "Next Day 12:00", ShippedDate = new DateTime(2016, 11, 17), DeliveredDate = new DateTime(2016, 11, 18), RecipientName = "Daisy Chan", NumberOfPackages = 1, Origin = "Hong Kong", Destination = "Kashi", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 6, ReferenceNumber = "", ServiceType = "Ground", ShippedDate = new DateTime(2016, 12, 16), DeliveredDate = new DateTime(2016, 12, 15), RecipientName = "Harry Lee", NumberOfPackages = 1, Origin = "Hong Kong", Destination = "Harbin", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 7, ReferenceNumber = "45236", ServiceType = "2nd Day", ShippedDate = new DateTime(2017, 01, 14), DeliveredDate = new DateTime(2017, 01, 16), RecipientName = "John Wong", NumberOfPackages = 2, Origin = "Hong Kong", Destination = "Changchun", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 8, ReferenceNumber = "", ServiceType = "Next Day", ShippedDate = new DateTime(2016, 10, 19), DeliveredDate = new DateTime(2016, 10, 20), RecipientName = "Lisa Li", NumberOfPackages = 1, Origin = "Beijing", Destination = "Haikou", Status = "Delivered", ShippingAccountId = 2 },
                new Shipment { WaybillId = 9, ReferenceNumber = "", ServiceType = "Same Day", ShippedDate = new DateTime(2016, 11, 04), DeliveredDate = new DateTime(2016, 11, 05), RecipientName = "Yolanda Yu", NumberOfPackages = 1, Origin = "Beijing", Destination = "Hangzhou", Status = "Delivered", ShippingAccountId = 2 },
                new Shipment { WaybillId = 10, ReferenceNumber = "", ServiceType = "Next Day", ShippedDate = new DateTime(2017, 02, 02), DeliveredDate = new DateTime(2017, 02, 03), RecipientName = "Yolanda Yu", NumberOfPackages = 2, Origin = "Beijing", Destination = "Jinan", Status = "Delivered", ShippingAccountId = 2 },
                new Shipment { WaybillId = 11, ReferenceNumber = "66543", ServiceType = "Ground", ShippedDate = new DateTime(2017, 01, 23), DeliveredDate = new DateTime(2017, 01, 26), RecipientName = "Arnold Au", NumberOfPackages = 3, Origin = "Beijing", Destination = "Guangzhou", Status = "Delivered", ShippingAccountId = 2 },
                new Shipment { WaybillId = 12, ReferenceNumber = "", ServiceType = "Next Day 12:00", ShippedDate = new DateTime(2016, 12, 18), DeliveredDate = new DateTime(2016, 12, 19), RecipientName = "Andrew Li", NumberOfPackages = 1, Origin = "Nanjing", Destination = "Beijing", Status = "Delivered", ShippingAccountId = 2 },
                new Shipment { WaybillId = 13, ReferenceNumber = "", ServiceType = "2nd Day", ShippedDate = new DateTime(2017, 01, 07), DeliveredDate = new DateTime(2017, 01, 09), RecipientName = "Amelia Auyeung", NumberOfPackages = 1, Origin = "Nanjing", Destination = "Kunming", Status = "Delivered", ShippingAccountId = 2 },
                new Shipment { WaybillId = 14, ReferenceNumber = "887564", ServiceType = "Next Day 15:00", ShippedDate = new DateTime(2017, 02, 02), DeliveredDate = new DateTime(2017, 02, 03), RecipientName = "Amanda Au", NumberOfPackages = 2, Origin = "Nanjing", Destination = "Beijing", Status = "Delivered", ShippingAccountId = 2 },
                new Shipment { WaybillId = 15, ReferenceNumber = "", ServiceType = "Ground", ShippedDate = new DateTime(2017, 01, 13), DeliveredDate = new DateTime(2017, 01, 20), RecipientName = "John Wong", NumberOfPackages = 1, Origin = "Hong Kong", Destination = "Nanning", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 16, ReferenceNumber = "348712", ServiceType = "Next Day 12:00", ShippedDate = new DateTime(2016, 12, 03), DeliveredDate = new DateTime(2016, 12, 04), RecipientName = "Derek Chan", NumberOfPackages = 5, Origin = "Haikou", Destination = "Hong Kong", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 17, ReferenceNumber = "", ServiceType = "2nd Day", ShippedDate = new DateTime(2017, 02, 10), DeliveredDate = new DateTime(2017, 02, 12), RecipientName = "Kelly Kwong", NumberOfPackages = 6, Origin = "Hong Kong", Destination = "Golmud", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 18, ReferenceNumber = "", ServiceType = "Same Day", ShippedDate = new DateTime(2017, 01, 18), DeliveredDate = new DateTime(2017, 01, 18), RecipientName = "Wendy Wang", NumberOfPackages = 4, Origin = "Hong Kong", Destination = "Hohhot", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 19, ReferenceNumber = "", ServiceType = "2nd Day", ShippedDate = new DateTime(2017, 02, 06), DeliveredDate = new DateTime(2017, 02, 08), RecipientName = "Larry Leung", NumberOfPackages = 2, Origin = "Guangzhou", Destination = "Hong Kong", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 20, ReferenceNumber = "22233398", ServiceType = "Next Day 15:00", ShippedDate = new DateTime(2016, 10, 09), DeliveredDate = new DateTime(2016, 10, 10), RecipientName = "Larry Leung", NumberOfPackages = 1, Origin = "Beijing", Destination = "Hong Kong", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 21, ReferenceNumber = "", ServiceType = "Same Day", ShippedDate = new DateTime(2016, 12, 04), DeliveredDate = new DateTime(2016, 12, 04), RecipientName = "Vincent Zhang", NumberOfPackages = 2, Origin = "Hulun Buir", Destination = "Lhasa", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 22, ReferenceNumber = "336723", ServiceType = "Ground", ShippedDate = new DateTime(2017, 02, 08), DeliveredDate = new DateTime(2017, 02, 10), RecipientName = "Sarah So", NumberOfPackages = 1, Origin = "Beijing", Destination = "Beijing", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 23, ReferenceNumber = "", ServiceType = "Next Day 15:00", ShippedDate = new DateTime(2016, 10, 23), DeliveredDate = new DateTime(2016, 10, 24), RecipientName = "Harry Ho", NumberOfPackages = 2, Origin = "Hefei", Destination = "Beijing", Status = "Delivered", ShippingAccountId = 1 },
                new Shipment { WaybillId = 24, ReferenceNumber = "", ServiceType = "Ground", ShippedDate = new DateTime(2017, 01, 15), DeliveredDate = new DateTime(2017, 01, 19), RecipientName = "Peter Pang", NumberOfPackages = 3, Origin = "Beijing", Destination = "Lhasa", Status = "Delivered", ShippingAccountId = 2 },
                new Shipment { WaybillId = 25, ReferenceNumber = "386456", ServiceType = "Same Day", ShippedDate = new DateTime(2017, 01, 05), DeliveredDate = new DateTime(2017, 01, 05), RecipientName = "Jerry Jia", NumberOfPackages = 1, Origin = "Beijing", Destination = "Hangzhou", Status = "Delivered", ShippingAccountId = 2 }
            );

           
           
           
           
           
           /***** Sample data*****/
            context.PersonalShippingAccounts.AddOrUpdate(
               p => p.ShippingAccountId,
               new PersonalShippingAccount { ShippingAccountId = unchecked((int)1111111111111111), FirstName = "Vicent", LastName = "Au", PhoneNumber = "85229649911", Email = "comp3111_team109@cse.ust.hk", Building = "Flat A 20/F Tower A Galaxia", Street = "275 Fung Tak Road", City = "Hong Kong", ProvinceCode = "HK", CreditCardType = "Visa", CreditCardNumber = "4988233518923326", CreditCardSecurityNumber = "222", CreditCardCardholderName = "Vicent Au", CreditCardExpiryMonth = "02", CreditCardExpiryYear = "2020", UserName = "vincent" },
               new PersonalShippingAccount { ShippingAccountId = unchecked((int)2222222222222222), FirstName = "Monica", LastName = "Mok", PhoneNumber = "862167890123", Email = "comp3111_team109@cse.ust.hk", Building = "Flat A 15/F Tower 2 Golden Estate", Street = "12 Mandarin Drive", City = "Shanghai", ProvinceCode = "SH", PostalCode = "207345", CreditCardType = "MasterCard", CreditCardNumber = "5567890123456780", CreditCardSecurityNumber = "861", CreditCardCardholderName = "Monica Mok", CreditCardExpiryMonth = "10", CreditCardExpiryYear = "2021", UserName = "monica" }
               );
            context.BusinessShippingAccounts.AddOrUpdate(
               p => p.ShippingAccountId,
               new BusinessShippingAccount { ShippingAccountId = unchecked((int)3333333333333333), CompanyName = "iGear Computing", DepartmentName = "Sales", ContactPersonName = "Felix Fong", PhoneNumber = " 862722873754", Email = "comp3311_team109@cse.ust.hk", Street = "18 Huaubaishu Road", City = "Wuhan", ProvinceCode = "HB", PostalCode = "433456", CreditCardType = "America Experss", CreditCardNumber = "349293478845325", CreditCardSecurityNumber = "9251", CreditCardCardholderName = "Felix Fong", CreditCardExpiryMonth = "06", CreditCardExpiryYear = "2022", UserName = "igearcomp" }
               );

            context.Addresses.AddOrUpdate(p => p.AddressId, new Address { AddressId = 1, Building = "Flat A 20/F Tower A Galaxia", Street = "275 Fung Tak Road", City = "Hong Kong", ProvinceCode = "HK", ShippingAccountId = unchecked((int)1111111111111111), isRecipientAddress = false },
                new Address { AddressId = 2, Building = "Flat A 15/F Tower 2 Golden Estate", Street = "12 Mandarin Drive", City = "Shanghai", ProvinceCode = "SH", PostalCode = "207345", isRecipientAddress = true, ShippingAccountId = unchecked((int)1111111111111111) },
                new Address { AddressId = 3, Street = "333 Golden Terrace", City = "Lanzhou", ProvinceCode = "GS", ShippingAccountId = unchecked((int)1111111111111111), isRecipientAddress = true, PostalCode = "737373" },
                new Address { AddressId = 4, Street = "12 Blossom Drive", City = "Fuzhou", ProvinceCode = "JX", ShippingAccountId = unchecked((int)1111111111111111), isRecipientAddress = true, PostalCode = "356655" },
                new Address { AddressId = 5, Street = "18 Huaubaishu Road", City = "Wuhan", ProvinceCode = "HB", ShippingAccountId = unchecked((int)1111111111111111), isRecipientAddress = true, PostalCode = "433456" });

            context.Packages.AddOrUpdate(p => p.PackageId, new Package { PackageId = 1, ContentCurrency = "HKD", Description = "Correspondence", ValueOfContent = 10, WaybillId = unchecked((int)1111111111111111), PackageTypeSizeId = 1 },
                new Package { PackageId = 2, ContentCurrency = "HKD", Description = "Correspondence", ValueOfContent = 10, WaybillId = unchecked((int)1111111111111111), PackageTypeSizeId = 1 },
                new Package { PackageId = 3, ContentCurrency = "HKD", Description = "Apple iPad mini", ValueOfContent = 2600, WaybillId = unchecked((int)2222222222222222), EstimatedWeight = 0.4m, PackageTypeSizeId = 2, Weight = 0.5m },
                new Package { PackageId = 4, ContentCurrency = "HKD", Description = "Painting", ValueOfContent = 1000, WaybillId = unchecked((int)3333333333333333), EstimatedWeight = 0.6m, PackageTypeSizeId = 4, Weight = 0.5m },
                new Package { PackageId = 5, ContentCurrency = "HKD", Description = "Perfume", ValueOfContent = 1500, WaybillId = unchecked((int)3333333333333333), EstimatedWeight = 2.3m, PackageTypeSizeId = 5, Weight = 2.3m },
                new Package { PackageId = 6, ContentCurrency = "HKD", Description = "Manual", ValueOfContent = 50, WaybillId = unchecked((int)4444444444444444), PackageTypeSizeId = 1 },
                new Package { PackageId = 7, ContentCurrency = "HKD", Description = "Samples", ValueOfContent = 200, WaybillId = unchecked((int)4444444444444444), EstimatedWeight = 1.5m, PackageTypeSizeId = 2, Weight = 1.4m },
                new Package { PackageId = 8, ContentCurrency = "HKD", Description = "Samples", ValueOfContent = 200, WaybillId = unchecked((int)4444444444444444), EstimatedWeight = 4.6m, PackageTypeSizeId = 3, Weight = 4.6m },
                new Package { PackageId = 9, ContentCurrency = "HKD", Description = "Design specifications", ValueOfContent = 50, WaybillId = unchecked((int)4444444444444444), EstimatedWeight = 1.0m, PackageTypeSizeId = 4, Weight = 1.0m });

            context.Shipments.AddOrUpdate(
                p => p.WaybillId,
                new Shipment { WaybillId = unchecked((int)1111111111111111), SenderId = unchecked((int)1111111111111111), ServiceType = "Next Day 10:30", RecipientId = 1, RecipientName = "Monica Mok", DeliveryAddress = 1, Destination = "Shanghai", DeliveryEmailNotification = false, EmailAddress = "comp3111_team109@cse.ust.hk", isConfirmed = true, IsImmediatePickup = true, NumberOfPackages = 2, Origin = "Hong Kong", Packages = new List<Package> { new Package { PackageId = 1 }, new Package { PackageId = 2 } }, PhoneNumber = "862167890123", PickupAddress = 1, PickupEmailNotification = false, PickupTime = new DateTime(2017, 04, 06, 12, 55, 00), RecipientPaysShipment = true, RecipientPaysTaxesDuties = true, ShipmentFee = 280, TotalDuties = 0, TotalTaxes = 0, Status = "Delivered", ShippedDate = new DateTime(2017, 04, 06, 18, 05, 00), DeliveredDate = new DateTime(2017, 04, 07, 08, 48, 00) },
                new Shipment { WaybillId = unchecked((int)2222222222222222), SenderId = unchecked((int)1111111111111111), ServiceType = "2nd Day", DeliveredDate = new DateTime(2017, 04, 12, 10, 13, 00), DeliveryAddress = 3, DeliveryEmailNotification = false, Destination = "Lanzhou", EmailAddress = "comp3111_team109@cse.ust.hk", isConfirmed = true, IsImmediatePickup = true, NumberOfPackages = 1, Origin = "Hong Kong", Packages = new List<Package> { new Package { PackageId = 3 } }, PhoneNumber = "8693177770123", PickupAddress = 1, PickupEmailNotification = false, PickupTime = new DateTime(2017, 04, 10, 14, 35, 00), RecipientName = "George Guo", RecipientPaysShipment = false, RecipientPaysTaxesDuties = false, ShipmentFee = 50, ShippedDate = new DateTime(2017, 04, 11, 10, 18, 00), Status = "Delivered", TotalDuties = 100, TotalTaxes = 0, RecipientId = 2 },
                new Shipment { WaybillId = unchecked((int)3333333333333333), SenderId = unchecked((int)1111111111111111), ServiceType = "Same Day", DeliveredDate = new DateTime(2017, 04, 14, 16, 53, 00), DeliveryAddress = 4, DeliveryEmailNotification = false, Destination = "Fuzhou", EmailAddress = "comp3111_team109@cse.ust.hk", isConfirmed = true, IsImmediatePickup = true, NumberOfPackages = 2, Origin = "Hong Kong", Packages = new List<Package> { new Package { PackageId = 4 }, new Package { PackageId = 5 } }, PhoneNumber = "8659166660123", PickupAddress = 1, PickupEmailNotification = false, PickupTime = new DateTime(2017, 04, 14, 07, 30, 00), RecipientName = "Sammy So", RecipientPaysShipment = false, RecipientPaysTaxesDuties = false, ShipmentFee = 413, ShippedDate = new DateTime(2017, 04, 14, 10, 18, 00), Status = "Delivered", TotalDuties = 250, TotalTaxes = 125, RecipientId = 3 },
                new Shipment { WaybillId = unchecked((int)4444444444444444), SenderId = unchecked((int)1111111111111111), ServiceType = "Ground", DeliveryAddress = 5, DeliveryEmailNotification = false, Destination = "Wuhan", EmailAddress = "comp3111_team109@cse.ust.hk", isConfirmed = true, IsImmediatePickup = true, NumberOfPackages = 4, Origin = "Hong Kong", Packages = new List<Package> { new Package { PackageId = 6 }, new Package { PackageId = 7 }, new Package { PackageId = 8 }, new Package { PackageId = 9 } }, PhoneNumber = "8659166660123", PickupAddress = 1, PickupEmailNotification = false, PickupTime = new DateTime(2017, 05, 02, 08, 00, 00), RecipientName = "Felix Fong", RecipientPaysShipment = true, RecipientPaysTaxesDuties = true, ShipmentFee = 190, ShippedDate = new DateTime(2017, 05, 02, 10, 35, 00), Status = "Delivered", TotalDuties = 0, TotalTaxes = 0, CompanyName = "iGear Computing", DepartmentName = "Sales", RecipientId = 4 }
                );

            context.PersonalShippingAccounts.AddOrUpdate(
               p => p.ShippingAccountId,
               new PersonalShippingAccount { ShippingAccountId = unchecked((int)1111111111111111), Shipments = new List<Shipment> { new Shipment { WaybillId = unchecked((int)1111111111111111) }, new Shipment { WaybillId = unchecked((int)2222222222222222) }, new Shipment { WaybillId = unchecked((int)3333333333333333) }, new Shipment { WaybillId = unchecked((int)444444444444) } } }
               );

            context.TrackingSystemRecords.AddOrUpdate(p => p.TrackingSystemRecordId, new TrackingSystemRecord { TrackingSystemRecordId = 1, Activity = "Picked up", DateTimeOfRecord = new DateTime(2017, 04, 06, 13, 35, 00), DeliveredAt = "Front door", DeliveredTo = "Monica Mok", Location = "Hong Kong", Remarks = "Vehicle 34", Status = "Delivered", WaybillId = unchecked((int)1111111111111111) },
               new TrackingSystemRecord { TrackingSystemRecordId = 2, Activity = "At local sort facility", DateTimeOfRecord = new DateTime(2017, 04, 06, 16, 15, 00), DeliveredAt = "Front door", DeliveredTo = "Monica Mok", Location = "Tung Chung", Remarks = "", Status = "Delivered", WaybillId = unchecked((int)1111111111111111) },
               new TrackingSystemRecord { TrackingSystemRecordId = 3, Activity = "Left origin", DateTimeOfRecord = new DateTime(2017, 04, 06, 18, 05, 00), DeliveredAt = "Front door", DeliveredTo = "Monica Mok", Location = "HKIA", Remarks = "CX0123", Status = "Delivered", WaybillId = unchecked((int)1111111111111111) },
               new TrackingSystemRecord { TrackingSystemRecordId = 4, Activity = "At local sort facility", DateTimeOfRecord = new DateTime(2017, 04, 06, 20, 18, 00), DeliveredAt = "Front door", DeliveredTo = "Monica Mok", Location = "Pudong", Remarks = "", Status = "Delivered", WaybillId = unchecked((int)1111111111111111) },
               new TrackingSystemRecord { TrackingSystemRecordId = 5, Activity = "On vehicle for delivery", DateTimeOfRecord = new DateTime(2017, 04, 07, 06, 38, 00), DeliveredAt = "Front door", DeliveredTo = "Monica Mok", Location = "Pudong", Remarks = "Vehicle 1032", Status = "Delivered", WaybillId = unchecked((int)1111111111111111) },
               new TrackingSystemRecord { TrackingSystemRecordId = 6, Activity = "Delivered", DateTimeOfRecord = new DateTime(2017, 04, 07, 08, 48, 00), Location = "Shanghai", Remarks = "", DeliveredAt = "Front door", DeliveredTo = "Monica Mok", Status = "Delivered", WaybillId = unchecked((int)1111111111111111) },

               new TrackingSystemRecord { TrackingSystemRecordId = 7, Activity = "Picked up", DateTimeOfRecord = new DateTime(2017, 04, 10, 16, 45, 00), Location = "Hong Kong", Remarks = "Vehicle 12", DeliveredAt = "Front door", DeliveredTo = "George Guo", Status = "Delivered", WaybillId = unchecked((int)2222222222222222) },
               new TrackingSystemRecord { TrackingSystemRecordId = 8, Activity = "At local sort facility", DateTimeOfRecord = new DateTime(2017, 04, 10, 20, 10, 00), Location = "Tung Chung", Remarks = "", DeliveredAt = "Front door", DeliveredTo = "George Guo", Status = "Delivered", WaybillId = unchecked((int)2222222222222222) },
               new TrackingSystemRecord { TrackingSystemRecordId = 9, Activity = "Left origin", DateTimeOfRecord = new DateTime(2017, 04, 11, 10, 18, 00), Location = "HKIA", Remarks = "KA3845", DeliveredAt = "Front door", DeliveredTo = "George Guo", Status = "Delivered", WaybillId = unchecked((int)2222222222222222) },
               new TrackingSystemRecord { TrackingSystemRecordId = 10, Activity = "At local sort facility", DateTimeOfRecord = new DateTime(2017, 04, 11, 15, 28, 00), Location = "Lanzhou", Remarks = "", DeliveredAt = "Front door", DeliveredTo = "George Guo", Status = "Delivered", WaybillId = unchecked((int)2222222222222222) },
               new TrackingSystemRecord { TrackingSystemRecordId = 11, Activity = "On vehicle for delivery", DateTimeOfRecord = new DateTime(2017, 04, 12, 07, 38, 00), Location = "Lanzhou", Remarks = "Vehicle 82", DeliveredAt = "Front door", DeliveredTo = "George Guo", Status = "Delivered", WaybillId = unchecked((int)2222222222222222) },
               new TrackingSystemRecord { TrackingSystemRecordId = 12, Activity = "Delivered", DateTimeOfRecord = new DateTime(2017, 04, 12, 10, 13, 00), Location = "Lanzhou", Remarks = "", DeliveredAt = "Front door", DeliveredTo = "George Guo", Status = "Delivered", WaybillId = unchecked((int)2222222222222222) },


               new TrackingSystemRecord { TrackingSystemRecordId = 13, Activity = "Picked up", DateTimeOfRecord = new DateTime(2017, 04, 14, 07, 55, 00), Location = "Hong Kong", Remarks = "Vehicle 13", DeliveredAt = "Front door", DeliveredTo = "Sammy So", Status = "Delivered", WaybillId = unchecked((int)3333333333333333) },
               new TrackingSystemRecord { TrackingSystemRecordId = 14, Activity = "At local sort facility", DateTimeOfRecord = new DateTime(2017, 04, 14, 09, 08, 00), Location = "Tung Chung", Remarks = "", DeliveredAt = "Front door", DeliveredTo = "Sammy So", Status = "Delivered", WaybillId = unchecked((int)3333333333333333) },
               new TrackingSystemRecord { TrackingSystemRecordId = 15, Activity = "Left origin", DateTimeOfRecord = new DateTime(2017, 04, 14, 10, 18, 00), Location = "HKIA", Remarks = "KA3845", DeliveredAt = "Front door", DeliveredTo = "Sammy So", Status = "Delivered", WaybillId = unchecked((int)3333333333333333) },
               new TrackingSystemRecord { TrackingSystemRecordId = 16, Activity = "At local sort facility", DateTimeOfRecord = new DateTime(2017, 04, 14, 15, 28, 00), Location = "Fuzhou", Remarks = "", DeliveredAt = "Front door", DeliveredTo = "Sammy So", Status = "Delivered", WaybillId = unchecked((int)3333333333333333) },
               new TrackingSystemRecord { TrackingSystemRecordId = 17, Activity = "On vehicle for delivery", DateTimeOfRecord = new DateTime(2017, 04, 14, 15, 50, 00), Location = "Fuzhou", Remarks = "Vehicle 82", DeliveredAt = "Front door", DeliveredTo = "Sammy So", Status = "Delivered", WaybillId = unchecked((int)3333333333333333) },
               new TrackingSystemRecord { TrackingSystemRecordId = 18, Activity = "Delivered", DateTimeOfRecord = new DateTime(2017, 04, 16, 16, 53, 00), Location = "Fuzhou", Remarks = "", DeliveredAt = "Front door", DeliveredTo = "Sammy So", Status = "Delivered", WaybillId = unchecked((int)3333333333333333) },

               new TrackingSystemRecord { TrackingSystemRecordId = 19, Activity = "Picked up", DateTimeOfRecord = new DateTime(2017, 05, 02, 08, 30, 00), Location = "Hong Kong", Remarks = "Vehicle 12", DeliveredAt = "", DeliveredTo = "", Status = "Picked up", WaybillId = unchecked((int)4444444444444444) },
               new TrackingSystemRecord { TrackingSystemRecordId = 20, Activity = "At local sort facility", DateTimeOfRecord = new DateTime(2017, 05, 02, 10, 00, 00), Location = "Shatin", Remarks = "", DeliveredAt = "", DeliveredTo = "", Status = "Picked up", WaybillId = unchecked((int)4444444444444444) },
               new TrackingSystemRecord { TrackingSystemRecordId = 21, Activity = "Left origin", DateTimeOfRecord = new DateTime(2017, 05, 02, 10, 35, 00), Location = "Shatin", Remarks = "Vehicle 667", DeliveredAt = "", DeliveredTo = "", Status = "Picked up", WaybillId = unchecked((int)4444444444444444) }
               );

            /***** Sample data*****/
           
           
           
           
           
           
           
           
           
           
           
        }
    }
}
