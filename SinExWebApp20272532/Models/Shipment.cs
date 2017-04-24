using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinExWebApp20272532.Models
{
    [Table("Shipment")]
    public class Shipment
    {
        [Key]
        public virtual int                      WaybillId                   { get; set; }
        public virtual string                   ReferenceNumber             { get; set; }
        public virtual string                   ServiceType                 { get; set; }
        public virtual DateTime                 ShippedDate                 { get; set; }
        public virtual DateTime                 DeliveredDate               { get; set; }
        public virtual string                   RecipientName               { get; set; }
        public virtual int                      NumberOfPackages            { get; set; }
        public virtual ICollection<Package>     Packages                    { get; set; }
        public virtual string                   Origin                      { get; set; }
        public virtual string                   Destination                 { get; set; }
        public virtual string                   Status                      { get; set; }
        [ForeignKey("ShipmentPayer")]
        public virtual int                      ShipmentPayerId             { get; set; }
        public virtual ShippingAccount          ShipmentPayer               { get; set; }
        [ForeignKey("TaxesDutiesPayer")]
        public virtual int                      TaxesDutiesPayerId          { get; set; }
        public virtual ShippingAccount          TaxesDutiesPayer            { get; set; }
        public virtual int                      ShippingAccountId           { get; set; }
        public virtual ShippingAccount          ShippingAccount             { get; set; }
        public virtual bool                     PickupEmailNotification     { get; set; }
        public virtual bool                     DeliveryEmailNotification   { get; set; }
    }
}