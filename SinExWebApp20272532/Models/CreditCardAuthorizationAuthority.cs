using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinExWebApp20272532.Models
{
    public class CreditCardAuthorizationAuthority
    {
        public string Authorize(string CardType, int CardNumber, int SecurityNumber, string CardholderName, string CardExpiryMonth, string CardExpiryYear)
        {
            return ((new System.Random().Next()) % 9999).ToString("D4");
        }
    }
}