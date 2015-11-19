using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jitter.Models;

namespace Jitter.Tests.Models
{
    [TestClass]
    public class JitterUserTests
    {
        [TestMethod]
        public void JitterUserEnsureICanCreateInstance()
        {
            JitterUser a_user = new JitterUser();
            Assert.IsNotNull(a_user);
        }

        [TestMethod]
        public void JitterUserEnsureJitterUserHasAllTheThings()
        {
            //arrange
            JitterUser a_user = new JitterUser();
            a_user.Handle = "adam12";
            a_user.FirstName = "Adam";
            a_user.LastName = "Sandler";
            a_user.Picture = "https://google.com";
            a_user.Description = "I am awesome!";
            //act

            //assert
            //Assert.AreEqual(1, a_user.JotId);
            Assert.AreEqual("adam1", a_user.Handle);
            Assert.AreEqual("Adam", a_user.FirstName);
            Assert.AreEqual("Sandler", a_user.LastName);
            Assert.AreEqual("I am awesome!", a_user.Description);
            Assert.AreEqual("https://google.com", a_user.Picture);
        }
    }
}
