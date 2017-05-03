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
        // Auto-generated 16-digit numeric
        [Key]
        [DisplayFormat(DataFormatString ="{0:D16}")]
        public virtual int                      WaybillId                   { get; set; }



        // Sender information
        [ForeignKey("Sender")]
        public virtual int                      SenderId                    { get; set; }
        public virtual ShippingAccount          Sender                      { get; set; }
        [StringLength(10)]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage ="The {0} field must contain only alphanumeric characters.")]
        public virtual string                   ReferenceNumber             { get; set; }
        


        // Recipient information
        [Required]
        [StringLength(70)]
        [Display(Name = "Recipient Name")]
        public virtual string                   RecipientName               { get; set; }
        [StringLength(40)]
        [Display(Name = "Company Name")]
        public virtual string                   CompanyName                 { get; set; }
        [StringLength(30)]
        [Display(Name = "Department Name")]
        public virtual string                   DepartmentName              { get; set; }
        public virtual Address                  DeliveryAddress             { get; set; }
        [Required]
        [StringLength(14, MinimumLength = 8)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        [Display(Name = "Phone Number")]
        public virtual string PhoneNumber { get; set; }
        [Required]
        [StringLength(30)]
        [EmailAddress()]
        [Display(Name = "Email Address")]
        public virtual string EmailAddress { get; set; }
        public virtual int                      RecipientId                 { get; set; }



        // Service information
        public virtual string                   ServiceType                 { get; set; }
        public virtual ShippingAccount          ShipmentPayer               { get; set; }
        public virtual ShippingAccount          TaxesDutiesPayer            { get; set; }



        // Packages information
        public virtual int                      NumberOfPackages            { get; set; }
        public virtual ICollection<Package>     Packages                    { get; set; }



        // Meta information
        public virtual int                      PickupArrangementId         { get; set; }
        public virtual decimal                  TotalTaxes                  { get; set; }
        public virtual decimal                  TotalDuties                 { get; set; }
        public virtual DateTime                 ShippedDate                 { get; set; }
        public virtual DateTime                 DeliveredDate               { get; set; }
        public virtual string                   Origin                      { get; set; }
        public virtual string                   Destination                 { get; set; }
        public virtual string                   Status                      { get; set; }
        public virtual bool                     PickupEmailNotification     { get; set; }
        public virtual bool                     DeliveryEmailNotification   { get; set; }
    }
}