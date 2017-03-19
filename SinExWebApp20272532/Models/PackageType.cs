using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SinExWebApp20272532.Models
{
    [Table("PackageType")]
    public class PackageType
    {
        public virtual int PackageTypeID { get; set; }
        [Display(Name = "Package Type")]
        public virtual string Type { get; set; }
        [Display(Name = "Description")]
        public virtual string Description { get; set; }
        public virtual ICollection<ServicePackageFee> ServicePackageFees { get; set; }
        public virtual ICollection<PackageTypeSize> PackageTypeSizes { get; set; }
    }
}