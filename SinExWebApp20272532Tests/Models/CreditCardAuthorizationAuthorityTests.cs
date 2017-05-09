using NUnit.Framework;
using SinExWebApp20272532.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinExWebApp20272532Tests.Models
{
    [TestFixture()]
    class CreditCardAuthorizationAuthorityTests
    {
        private static CreditCardAuthorizationAuthority instance = new CreditCardAuthorizationAuthority();

        [TestCase("American Express", 8888888888888888, 888, "Holder", "12", "22", true)]
        [TestCase("Diners Club", 8888888888888888, 888, "Holder", "12", "22", true)]
        [TestCase("Discover", 8888888888888888, 888, "Holder", "12", "22", true)]
        [TestCase("MasterCard", 8888888888888888, 888, "Holder", "12", "22", true)]
        [TestCase("UnionPay", 8888888888888888, 888, "Holder", "12", "22", true)]
        [TestCase("Visa", 8888888888888888, 888, "Holder", "12", "22", true)]
        [TestCase("NotValid", 8888888888888888, 888, "Holder", "12", "22", false)]
        [TestCase("Visa", 8888888888888888, 888, "Holder", "13", "22", false)]
        [TestCase("Visa", 8888888888888888, 888, "Holder", "0", "22", false)]
        public void CreditCardAuthorizationAuthorityTest(string CardType, Int64 CardNumber, int SecurityNumber, string CardholderName, string CardExpiryMonth, string CardExpiryYear, bool validCase)
        {
            try
            {
                instance.Authorize(CardType, CardNumber, SecurityNumber, CardholderName, CardExpiryMonth, CardExpiryYear);
                if (!validCase)
                {
                    Assert.Fail("Invalid result expected");
                }
            }
            catch (CreditCardInvalidException e)
            {
                if (validCase)
                    Assert.Fail(e.Message);
                return;
            }
        }
    }
}
