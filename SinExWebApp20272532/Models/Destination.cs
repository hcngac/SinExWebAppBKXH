using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinExWebApp20272532.Models
{
    [Table("Destination")]
    public class Destination
    {
        public virtual int DestinationID { get; set; }
        public virtual string City { get; set; }
        public virtual string ProvinceCode { get; set; }
        [ForeignKey("Currency")]
        public virtual string CurrencyCode { get; set; }
        public virtual Currency Currency { get; set; }
    }
}