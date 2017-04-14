using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SinExWebApp20272532.Models
{
    [Table("Currency")]
    public class Currency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Currency Code")]
        public virtual string CurrencyCode { get; set; }
        [Display(Name = "Exchange Rate")]
        public virtual decimal ExchangeRate { get; set; }
        public virtual ICollection<Destination> Destinations { get; set; }

        public static List<Currency> getCachedList()
        {
            Cache Cache = HttpRuntime.Cache;
            List<Currency> currencyList = Cache["currencyList"] as List<Currency>;
            if (currencyList == null)
            {
                SinExDatabaseContext db = new SinExDatabaseContext();
                currencyList = db.Currencies.ToList();
            }
            return currencyList;
        }

        public static SelectList getSelectList()
        {
            List<Currency> currencyList = getCachedList();
            List<string> currencyCodeList;
            if (currencyList == null)
            {
                currencyCodeList = new List<string>();
            }
            else
            {
                currencyCodeList = currencyList.Select(a => a.CurrencyCode).ToList();
            }
            currencyCodeList.Insert(0,"Please Select");
            return new SelectList(currencyCodeList);
        }
    }
}