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
    [Table("ServiceType")]
    public class ServiceType
    {
        public virtual int ServiceTypeID { get; set; }
        [Display(Name = "Service Type")]
        public virtual string Type { get; set; }
        [Display(Name = "Cut Off Time")]
        public virtual string CutoffTime { get; set; }
        [Display(Name = "Delivery Time")]
        public virtual string DeliveryTime { get; set; }
        public virtual ICollection<ServicePackageFee> ServicePackageFees { get; set; }

        public static List<ServiceType> getCachedList()
        {
            Cache Cache = new Cache();
            SinExDatabaseContext db = new SinExDatabaseContext();
            List<ServiceType> serviceTypeList = Cache["serviceTypeList"] as List<ServiceType>;
            if (serviceTypeList == null)
            {
                serviceTypeList = db.ServiceTypes.ToList();
            }
            return serviceTypeList;
        }

        public static SelectList getSelectList()
        {
            Cache Cache = new Cache();
            List<ServiceType> serviceTypeList = Cache["serviceTypeList"] as List<ServiceType>;
            List<string> serviceTypeNameList;
            if (serviceTypeList == null)
            {
                serviceTypeNameList = new List<string>();
            }
            else
            {
                serviceTypeNameList = serviceTypeList.Select(a => a.Type).ToList();
            }
            serviceTypeNameList.Insert(0, "Please Select");
            return new SelectList(serviceTypeNameList);
        }
    }
}