using NUnit.Framework;
using SinExWebApp20272532.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SinExWebApp20272532.Controllers.Tests
{
    [TestFixture()]
    public class ServicePackageFeesControllerTests
    {
        [TestCase("SameDay","Envelop", null,"HKD",160 )]

        public void IndexTest(string ServiceType, string PackageType, decimal? Weight, string Currency, decimal actionResult)
        {/*
            ServicePackageFeesController servicePackageFeesController = new ServicePackageFeesController();

            ActionResult a = servicePackageFeesController.Index(ServiceType, PackageType, Weight, Currency, false);

            Assert.IsInstanceOf(a, typeof(RedirectToRouteResult));

            RedirectToRouteResult routeResult = a as RedirectToRouteResult;

            Assert.AreEqual(routeResult.RouteValues["action"], result);

            Assert.Equals(servicePackageFeesController.Index(ServiceType, PackageType, Weight, Currency, false),actionResult);
        */}

        /*
        [Test()]
        public void IndexTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void DetailsTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void CreateTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void CreateTest1()
        {
            Assert.Fail();
        }

        [Test()]
        public void EditTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void EditTest1()
        {
            Assert.Fail();
        }

        [Test()]
        public void DeleteTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void DeleteConfirmedTest()
        {
            Assert.Fail();
        }
        */
    }
}