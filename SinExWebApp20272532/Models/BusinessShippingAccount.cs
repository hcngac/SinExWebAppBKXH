using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SinExWebApp20272532.Models
{
    public class BusinessShippingAccount : ShippingAccount
    {
        [Required]
        [StringLength(70)]
        [Display(Name = "Contact Person Name")]
        public virtual string ContactPersonName { get; set; }
        [Required]
        [StringLength(40)]
        [Display(Name = "Company Name")]
        public virtual string CompanyName { get; set; }
        [StringLength(30)]
        [Display(Name = "Department Name")]
        public virtual string DepartmentName { get; set; }
    }
}