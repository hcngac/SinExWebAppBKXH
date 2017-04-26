using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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
    }
}