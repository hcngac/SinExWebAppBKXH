﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SinExWebApp20272532;
using SinExWebApp20272532.Controllers;

namespace SinExWebApp20272532.Controllers.Tests
{
    [TestFixture()]
    public class BaseControllerTests
    {
        [TestCase(12, 1.0, 12 * 1.0)]
        [TestCase(15, 1.13, 15 * 1.13)]
        [TestCase(18, 1.16, 18 * 1.16)]
        [TestCase(21, 4.56, 21 * 4.56)]
        public void CurrencyExchangeTest(decimal source, decimal rate, decimal result)
        {
            BaseController baseController = new BaseController();

            Assert.Equals(source * rate, baseController.CurrencyExchange(source, 1, rate));
        }
    }
}