using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinExWebApp20272532.Models
{
    [Table("Address")]
    public class Address
    {
        public virtual int AddressId { get; set; }
        public virtual string AddressName { get; set; }

        [StringLength(50)]
        [Display(Name = "Building")]
        public virtual string Building { get; set; }

        [Required]
        [StringLength(35)]
        [Display(Name = "Street")]
        public virtual string Street { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "City")]
        public virtual string City { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "Province")]
        public virtual string ProvinceCode { get; set; }

        [StringLength(6, MinimumLength = 5)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        [Display(Name = "Postal Code")]
        public virtual string PostalCode { get; set; }

        [ForeignKey("ShippingAccount")]
        public virtual int ShippingAccountId { get; set; }
        public virtual ShippingAccount ShippingAccount { get; set; }

        public virtual bool isRecipientAddress { get; set; }

        public static SelectList GetSelectList(int shippingAccountId, bool isRA)
        {
            var db = new SinExDatabaseContext();
            var addressListQuery = from s in db.Addresses
                              where s.ShippingAccountId == shippingAccountId
                              where s.isRecipientAddress == isRA
                              select new SelectListItem
                              {
                                  Value = s.AddressId.ToString(),
                                  Text = s.AddressName,
                                  Selected = false,
                              };
            return new SelectList(addressListQuery.ToList(),"Value","Text");
        }
    }
}