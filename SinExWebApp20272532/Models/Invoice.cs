using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20272532.Models
{
    [Table("Invoice")]
    public class Invoice
    {
        public virtual int InvoiceId { get; set; }
        public virtual int WaybillId { get; set; }
        public virtual DateTime ShipDate { get; set; }
        public virtual string RecipientName { get; set; }
        public virtual string Origin { get; set; }
        public virtual string Destination { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual decimal InvoiceAmount { get; set; }
    }
}