using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinExWebApp20272532.Models
{
    [Table("Pickup")]
    public class Pickup
    {
        public virtual int PickupId { get; set; }
        public virtual DateTime PickupTime { get; set; }
        public virtual bool IsImmediatePickup { get; set; }
        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}