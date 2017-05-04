using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SinExWebApp20272532.Models
{
    [Table("ServicePackageFee")]
    public class ServicePackageFee
    {
        public virtual int ServicePackageFeeID { get; set; }
        [Display(Name = "Fee")]
        public virtual decimal Fee { get; set; }
        [Display(Name = "Minimum Fee")]
        public virtual decimal MinimumFee { get; set; }
        public virtual decimal Penalty { get; set; }
        public virtual int PackageTypeID { get; set; }
        public virtual int ServiceTypeID { get; set; }
        public virtual PackageType PackageType { get; set; }
        public virtual ServiceType ServiceType { get; set; }
    }
}