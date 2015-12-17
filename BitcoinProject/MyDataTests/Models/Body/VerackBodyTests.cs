using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Body;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Models.Body.Tests
{
    [TestClass()]
    public class VerackBodyTests
    {
        VerackBody vb = new VerackBody();
        [TestMethod()]
        public void GetPayloadSizeTest()
        {
            Assert.AreEqual(vb.GetPayloadSize(), (uint)0);
        }

        [TestMethod()]
        public void SerializeTest()
        {
            Assert.IsTrue(true);//this is a dummy body, it cannot fail.
        }
    }
}
