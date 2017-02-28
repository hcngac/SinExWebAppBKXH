using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinExWebApp20272532.Models
{
    [Table("ShippingAccount")]
    public abstract class ShippingAccount
    {
        public virtual int ShippingAccountId { get; set; }

        // Mailing Address Information.
        [StringLength(50)]
        public virtual string Building { get; set; }

        [Required]
        [StringLength(35)]
        public virtual string Street { get; set; }

        [Required]
        [StringLength(25)]
        public virtual string City { get; set; }

        [Required]
        [StringLength(2)]
        public virtual string ProvinceCode { get; set; }

        [StringLength(6,MinimumLength =5)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        public virtual string PostalCode { get; set; }

        // Credit Card Information.
        [Required]
        public virtual string CreditCardType { get; set; }

        [Required]
        [StringLength(19,MinimumLength =14)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        public virtual string CreditCardNumber { get; set; }

        [Required]
        [StringLength(4,MinimumLength =3)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        public virtual string CreditCardSecurityNumber { get; set; }

        [Required]
        [StringLength(70)]
        public virtual string CreditCardCardholderName { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        public virtual string CreditCardExpiryMonth { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        public virtual string CreditCardExpiryYear { get; set; }

        // General Information.

        [Required]
        [StringLength(14,MinimumLength =8)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The field {0} must be a number.")]
        public virtual string PhoneNumber { get; set; }

        [Required]
        [StringLength(30)]
        [EmailAddress()]
        public virtual string EmailAddress { get; set; }
    }
}