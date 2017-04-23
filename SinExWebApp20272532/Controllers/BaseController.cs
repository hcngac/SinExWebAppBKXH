using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SinExWebApp20272532.Controllers
{
    public class BaseController : Controller
    {
        public decimal CurrencyExchange(decimal sourceAmount, decimal sourceRate, decimal exchangeRate)
        {
            decimal target = 0;

            target = sourceAmount * exchangeRate / sourceRate;

            return target;
        }
    }
}