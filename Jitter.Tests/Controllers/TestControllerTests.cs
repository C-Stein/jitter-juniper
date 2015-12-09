using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Jitter.Controllers;

namespace Jitter.Tests.Controllers
{
    [TestClass]
    public class TestControllerTests
    {
        [TestMethod]
        public void TestControllerEnsureICanCallGetAction()
        {
            TestController my_controller = new TestController();
            string expected_output = "Hello, It's Me!";
            string actual_output = my_controller.Get();
            Assert.AreEqual(expected_output, actual_output);
        }
    }
}
