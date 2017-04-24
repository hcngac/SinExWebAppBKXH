using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinExWebApp20272532.Models
{
    [Table("Package")]
    public class Package
    {
        [Key]
        public virtual int          PackageId       { get; set; }
        [ForeignKey("Shipment")]
        public virtual int          WaybillId       { get; set; }
        public virtual Shipment     Shipment       { get; set; }
        public virtual int PackageTypeID { get; set; }
        public virtual PackageType  PackageType     { get; set; }
        public virtual string       Description     { get; set; }
        //ValueOfContent is only stored in CNY
        public virtual decimal      ValueOfContent  { get; set; }
        public virtual decimal      EstimatedWeight { get; set; }
        public virtual decimal      Weight          { get; set; }
    }
}