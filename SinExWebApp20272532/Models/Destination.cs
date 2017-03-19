using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SinExWebApp20272532.Models
{
    [Table("Destination")]
    public class Destination
    {
        public virtual int DestinationID { get; set; }
        [Display(Name = "City")]
        public virtual string City { get; set; }
        [Display(Name = "Province Code")]
        public virtual string ProvinceCode { get; set; }
        [ForeignKey("Currency")]
        [Display(Name = "Currency Code")]
        public virtual string CurrencyCode { get; set; }
        public virtual Currency Currency { get; set; }
    }
}