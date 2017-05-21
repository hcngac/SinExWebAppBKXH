using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace SinExWebApp20272532.Models
{
    public class SinExDatabaseContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public SinExDatabaseContext() : base("name=SinExDatabaseContext")
        {
        }
        
        
        
        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.PackageType> PackageTypes { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.ServiceType> ServiceTypes { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.ServicePackageFee> ServicePackageFees { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.Destination> Destinations { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.Currency> Currencies { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.PackageTypeSize> PackageTypeSizes { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.Shipment> Shipments { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.Package> Packages { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.Address> Addresses { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.ShippingAccount> ShippingAccounts { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.Pickup> Pickups { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.TrackingSystemRecord> TrackingSystemRecords { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20272532.Models.Invoice> Invoices { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                    ); // Add the original exception as the innerException
            }
        }
    }
}
