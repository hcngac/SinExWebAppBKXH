using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinExWebApp20272532.Models
{
    public class CreditCardInvalidException : Exception
    {
        public CreditCardInvalidException(string message = "") : base(message)
        {

        }
    }
    public class CreditCardAuthorizationAuthority
    {
        public string Authorize(string CardType, Int64 CardNumber, int SecurityNumber, string CardholderName, string CardExpiryMonth, string CardExpiryYear)
        {
            if (
                int.Parse(CardExpiryMonth) < 1 ||
                int.Parse(CardExpiryMonth) > 12
                )
            {
                throw new CreditCardInvalidException("Invalid Expiry Month.");
            }
            if (
                    CardType != "American Express" &&
                    CardType != "Diners Club" &&
                    CardType != "Discover" &&
                    CardType != "MasterCard" &&
                    CardType != "UnionPay" &&
                    CardType != "Visa"
                )
            {
                throw new CreditCardInvalidException("Invalid Card Type.");
            }
            return ((new System.Random().Next()) % 9999).ToString("D4");
        }
    }
}