namespace SinExWebApp20272532.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<SinExWebApp20272532.Models.SinExDatabaseContext>
    {
        public Configuration()
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
                new ServicePackageFee { ServicePackageFeeID = 1, Fee = 160, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 1 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 2, Fee = 100, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 2 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 3, Fee = 100, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 3 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 4, Fee = 110, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 4 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 5, Fee = 110, MinimumFee = 160, ServiceTypeID = 1, PackageTypeID = 5 }, // Customer
                                                                                                                                      // Next Day 10:30
                new ServicePackageFee { ServicePackageFeeID = 6, Fee = 140, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 1 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 7, Fee = 90, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 2 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 8, Fee = 90, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 3 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 9, Fee = 99, MinimumFee = 100, ServiceTypeID = 2, PackageTypeID = 4 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 10, Fee = 99, MinimumFee = 140, ServiceTypeID = 2, PackageTypeID = 5 }, // Customer
                                                                                                                                      // Next Day 12:00
                new ServicePackageFee { ServicePackageFeeID = 11, Fee = 130, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 1 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 12, Fee = 80, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 2 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 13, Fee = 80, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 3 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 14, Fee = 88, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 4 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 15, Fee = 88, MinimumFee = 130, ServiceTypeID = 3, PackageTypeID = 5 }, // Customer
                                                                                                                                      // Next Day 15:00
                new ServicePackageFee { ServicePackageFeeID = 16, Fee = 120, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 1 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 17, Fee = 70, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 2 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 18, Fee = 70, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 3 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 19, Fee = 77, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 4 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 20, Fee = 77, MinimumFee = 120, ServiceTypeID = 4, PackageTypeID = 5 }, // Customer
                                                                                                                                      // 2nd Day
                new ServicePackageFee { ServicePackageFeeID = 21, Fee = 50, MinimumFee = 50, ServiceTypeID = 5, PackageTypeID = 1 }, // Envelope
                new ServicePackageFee { ServicePackageFeeID = 22, Fee = 50, MinimumFee = 50, ServiceTypeID = 5, PackageTypeID = 2 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 23, Fee = 50, MinimumFee = 50, ServiceTypeID = 5, PackageTypeID = 3 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 24, Fee = 55, MinimumFee = 55, ServiceTypeID = 5, PackageTypeID = 4 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 25, Fee = 55, MinimumFee = 55, ServiceTypeID = 5, PackageTypeID = 5 }, // Customer
                                                                                                                                     // Ground
                new ServicePackageFee { ServicePackageFeeID = 26, Fee = 25, MinimumFee = 25, ServiceTypeID = 6, PackageTypeID = 1 },// Envelope
                new ServicePackageFee { ServicePackageFeeID = 27, Fee = 25, MinimumFee = 25, ServiceTypeID = 6, PackageTypeID = 2 }, // Pak
                new ServicePackageFee { ServicePackageFeeID = 28, Fee = 25, MinimumFee = 25, ServiceTypeID = 6, PackageTypeID = 3 }, // Tube
                new ServicePackageFee { ServicePackageFeeID = 29, Fee = 30, MinimumFee = 30, ServiceTypeID = 6, PackageTypeID = 4 }, // Box
                new ServicePackageFee { ServicePackageFeeID = 30, Fee = 30, MinimumFee = 30, ServiceTypeID = 6, PackageTypeID = 5 }  // Customer
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
                new Destination { DestinationID = 2, ProvinceCode = "BJ", City = "Beijing" },
                new Destination { DestinationID = 3, ProvinceCode = "JL", City = "Changchun" },
                new Destination { DestinationID = 4, ProvinceCode = "HN", City = "Changsha" },
                new Destination { DestinationID = 5, ProvinceCode = "SC", City = "Chengdu" },
                new Destination { DestinationID = 6, ProvinceCode = "CQ", City = "Chongqing" },
                new Destination { DestinationID = 7, ProvinceCode = "JX", City = "Fuzhou" },
                new Destination { DestinationID = 8, ProvinceCode = "QH", City = "Golmud" },
                new Destination { DestinationID = 9, ProvinceCode = "GD", City = "Guangzhou" },
                new Destination { DestinationID = 10, ProvinceCode = "GZ", City = "Guiyang" },
                new Destination { DestinationID = 11, ProvinceCode = "HI", City = "Haikou" },
                new Destination { DestinationID = 12, ProvinceCode = "NM", City = "Hailar" },
                new Destination { DestinationID = 13, ProvinceCode = "ZJ", City = "Hangzhou" },
                new Destination { DestinationID = 14, ProvinceCode = "HL", City = "Harbin" },
                new Destination { DestinationID = 15, ProvinceCode = "AH", City = "Hefei" },
                new Destination { DestinationID = 16, ProvinceCode = "NM", City = "Hohhot" },
                new Destination { DestinationID = 17, ProvinceCode = "HK", City = "Hong Kong" },
                new Destination { DestinationID = 18, ProvinceCode = "NM", City = "Hulun Buir" },
                new Destination { DestinationID = 19, ProvinceCode = "SD", City = "Jinan" },
                new Destination { DestinationID = 20, ProvinceCode = "XJ", City = "Kashi" },
                new Destination { DestinationID = 21, ProvinceCode = "YN", City = "Kunming" },
                new Destination { DestinationID = 22, ProvinceCode = "GS", City = "Lanzhou" },
                new Destination { DestinationID = 23, ProvinceCode = "XZ", City = "Lhasa" },
                new Destination { DestinationID = 24, ProvinceCode = "MC", City = "Macau" },
                new Destination { DestinationID = 25, ProvinceCode = "JX", City = "Nanchang" },
                new Destination { DestinationID = 26, ProvinceCode = "JS", City = "Nanjing" },
                new Destination { DestinationID = 27, ProvinceCode = "JX", City = "Nanning" },
                new Destination { DestinationID = 28, ProvinceCode = "HL", City = "Qiqihar" },
                new Destination { DestinationID = 29, ProvinceCode = "SH", City = "Shanghai" },
                new Destination { DestinationID = 30, ProvinceCode = "LN", City = "Shenyang" },
                new Destination { DestinationID = 31, ProvinceCode = "HE", City = "Shijiazhuang" },
                new Destination { DestinationID = 32, ProvinceCode = "TW", City = "Taipei" },
                new Destination { DestinationID = 33, ProvinceCode = "SX", City = "Taiyuan" },
                new Destination { DestinationID = 34, ProvinceCode = "HE", City = "Tianjin" },
                new Destination { DestinationID = 35, ProvinceCode = "XJ", City = "Urumqi" },
                new Destination { DestinationID = 36, ProvinceCode = "HB", City = "Wuhan" },
                new Destination { DestinationID = 37, ProvinceCode = "SN", City = "Xi'an" },
                new Destination { DestinationID = 38, ProvinceCode = "QH", City = "Xining" },
                new Destination { DestinationID = 39, ProvinceCode = "NX", City = "Yinchuan" },
                new Destination { DestinationID = 40, ProvinceCode = "GS", City = "Yumen" },
                new Destination { DestinationID = 41, ProvinceCode = "HA", City = "Zhengzhou" }
                );

            context.PackageTypeSizes.AddOrUpdate(
                p => p.PackageTypeSizeID,
                new PackageTypeSize { PackageTypeSizeID = 1, Size = "250x300mm", WeightLimit = "Not Applicable", PackageTypeID = 1 },
                new PackageTypeSize { PackageTypeSizeID = 2, Size = "small - 350x400mm", WeightLimit = "5kg", PackageTypeID = 2 },
                new PackageTypeSize { PackageTypeSizeID = 3, Size = "large - 450x550mm", WeightLimit = "5kg", PackageTypeID = 2 },
                new PackageTypeSize { PackageTypeSizeID = 4, Size = "1000x80mm", WeightLimit = "Not Applicable", PackageTypeID = 3 },
                new PackageTypeSize { PackageTypeSizeID = 5, Size = "small - 300x250x150mm", WeightLimit = "10kg", PackageTypeID = 4 },
                new PackageTypeSize { PackageTypeSizeID = 6, Size = "medium - 400x350x250mm", WeightLimit = "20kg", PackageTypeID = 4 },
                new PackageTypeSize { PackageTypeSizeID = 7, Size = "large - 500x450x350mm", WeightLimit = "30kg", PackageTypeID = 4 }
                );

            /*
             */
        }
    }
}
