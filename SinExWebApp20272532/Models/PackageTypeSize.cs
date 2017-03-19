using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SinExWebApp20272532.Models
{
    [Table("PackageTypeSize")]
    public class PackageTypeSize
    {
        public virtual int PackageTypeSizeID { get; set; }
        [Display(Name = "Package Size")]
        public virtual string Size { get; set; }
        [Display(Name = "Weight Limit")]
        public virtual int WeightLimit { get; set; }
        public virtual int PackageTypeID { get; set; }
        public virtual PackageType PackageType { get; set; }

    }
}