using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SinExWebApp20272532.Validators;
using System.Web.Mvc;

namespace SinExWebApp20272532.Models
{
    [Table("ShippingAccount")]
    public abstract class ShippingAccount
    {
        public virtual int ShippingAccountId { get; set; }

        // Mailing Address Information.
        [StringLength(50)]
        [Display(Name ="Building")]
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

        [StringLength(6,MinimumLength =5)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        [Display(Name = "Postal Code")]
        public virtual string PostalCode { get; set; }

        // Credit Card Information.
        [Required]
        [CreditCardType]
        [Display(Name = "Type")]
        public virtual string CreditCardType { get; set; }

        [Required]
        [StringLength(19,MinimumLength =14)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        [Display(Name = "Card Number")]
        public virtual string CreditCardNumber { get; set; }

        [Required]
        [StringLength(4,MinimumLength =3)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        [Display(Name = "Security Number")]
        public virtual string CreditCardSecurityNumber { get; set; }

        [Required]
        [StringLength(70)]
        [Display(Name = "Cardholder Name")]
        public virtual string CreditCardCardholderName { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        [StringLength(2)]
        [Range(1,12)]
        [Display(Name = "Expiry Month")]
        public virtual string CreditCardExpiryMonth { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        [StringLength(2)]
        [Display(Name = "Expiry Year")]
        public virtual string CreditCardExpiryYear { get; set; }

        // General Information.

        [Required]
        [StringLength(14,MinimumLength =8)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        [Display(Name = "Phone Number")]
        public virtual string PhoneNumber { get; set; }

        [Required]
        [StringLength(30)]
        [EmailAddress()]
        [Display(Name ="Email Address")]
        public virtual string Email { get; set; }

        [StringLength(10)]
        public virtual string UserName { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }

        public static List<ShippingAccount> getCachedList()
        {
            Cache Cache = HttpRuntime.Cache;
            List<ShippingAccount> shippingAccountList = Cache["shippingAccountList"] as List<ShippingAccount>;
            if (shippingAccountList == null)
            {
                SinExDatabaseContext db = new SinExDatabaseContext();
                shippingAccountList = db.ShippingAccounts.ToList();
            }
            return shippingAccountList;
        }

        public static SelectList getSelectList()
        {
            List<ShippingAccount> shippingAccountList = getCachedList();
            List<string> shippingAccountIdList;
            if (shippingAccountList == null)
            {
                shippingAccountIdList = new List<string>();
            }
            else
            {
                List<int> raw_shippingAccountIdList = shippingAccountList.Select(a => a.ShippingAccountId).ToList();
                shippingAccountIdList = raw_shippingAccountIdList.ConvertAll<string>(delegate(int i) { return i.ToString(); });
            }
            shippingAccountIdList.Insert(0, "Please Select");
            return new SelectList(shippingAccountIdList);
        }
    }
}