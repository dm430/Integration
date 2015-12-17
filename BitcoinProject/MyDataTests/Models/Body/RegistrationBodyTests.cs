using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Body;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Models.Body.Tests
{
    [TestClass()]
    public class RegistrationBodyTests
    {
        RegistrationBody rb = new RegistrationBody() { UserHandle = "power ranger", PublicKey = "ghf5dg5f7f76gtf5dr5erdftfgvbnhju", Timestamp = 1234};
        [TestMethod()]
        public void GetPayloadSizeTest()
        {
            Assert.AreEqual(rb.GetPayloadSize(), (uint)(rb.UserHandle.Length + rb.PublicKey.Length + 12));
        }

        [TestMethod()]
        public void SerializeTest()
        {
            byte[] data = rb.Serialize();
            RegistrationBody rb2 = new RegistrationBody();
            rb2.Inflate(new MemoryStream(data));
            Assert.IsTrue(rb.Equals(rb2));
        }
    }
}
