using NUnit.Framework;
using SinExWebApp20272532.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinExWebApp20272532.Controllers.Tests
{
    [TestFixture()]
    public class BaseControllerTests
    {
        [Test()]
        public void CurrencyExchangeTest()
        {
            BaseController baseController = new BaseController();
            int[] sourceAmounts = { 12,15,18,21};

            decimal CNYRate,HKDRate,MOPRate,TWDRate;
            CNYRate = 1.0M;
            HKDRate = 1.13M;
            MOPRate = 1.16M;
            TWDRate = 4.56M;

            Assert.Equals(12, baseController.CurrencyExchange(sourceAmounts[0], 1, CNYRate));
            Assert.Equals(15 * HKDRate, baseController.CurrencyExchange(sourceAmounts[1], 1, HKDRate));
            Assert.Equals(18 * MOPRate, baseController.CurrencyExchange(sourceAmounts[2], 1, MOPRate));
            Assert.Equals(21 * TWDRate, baseController.CurrencyExchange(sourceAmounts[3], 1, TWDRate));
        }
    }
}