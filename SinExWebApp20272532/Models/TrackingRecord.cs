using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20272532.Models
{
    [Table("TrackingRecord")]
    public class TrackingRecord
    {
        public virtual int TrackingRecordId { get; set; }
        public virtual int WaybillId { get; set; }
        public virtual DateTime DateTimeOfRecord { get; set; }
        public virtual string Activity { get; set; }
        public virtual string Location { get; set; }
        public virtual string Remarks { get; set; }
        public virtual string DeliveredTo { get; set; }
        public virtual string DeliveredAt { get; set; }
        public virtual string Status { get; set; }
    }
}