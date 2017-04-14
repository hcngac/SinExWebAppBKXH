using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

        public static List<PackageType> getCachedList()
        {
            Cache Cache = HttpRuntime.Cache;
            SinExDatabaseContext db = new SinExDatabaseContext();
            List<PackageType> packageTypeList = Cache["packageTypeList"] as List<PackageType>;
            if (packageTypeList == null)
            {
                packageTypeList = db.PackageTypes.ToList();
            }
            return packageTypeList;
        }

        public static SelectList getSelectList()
        {
            List<PackageType> packageTypeList = getCachedList();
            List<string> packageTypeNameList;
            if (packageTypeList == null)
            {
                packageTypeNameList = new List<string>();
            }
            else
            {
                packageTypeNameList = packageTypeList.Select(a => a.Type).ToList();
            }
            packageTypeNameList.Insert(0, "Please Select");
            return new SelectList(packageTypeNameList);
        }
    }
}