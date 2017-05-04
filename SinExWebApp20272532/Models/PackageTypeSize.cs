using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

        public static SelectList GetSelectList()
        {
            var db = new Models.SinExDatabaseContext();
            List<Object> PackageTypeSizeSelectList = new List<Object>();
            var PackageTypeSizeList = db.PackageTypeSizes.ToList();
            foreach (PackageTypeSize x in PackageTypeSizeList)
            {
                string name = x.PackageType.Type + " - " + x.Size;
                int value = x.PackageTypeSizeID;
                PackageTypeSizeSelectList.Add(new { Text = name, Value = value.ToString() });
            }

            return new SelectList(PackageTypeSizeSelectList,"Value","Text");
        }

    }


}