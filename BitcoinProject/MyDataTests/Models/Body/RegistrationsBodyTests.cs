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
    public class RegistrationsBodyTests
    {
        RegistrationsBody rb = new RegistrationsBody() { Registrations = 
            new List<RegistrationBody>() {
                new RegistrationBody() { UserHandle = "power ranger", PublicKey = "ghf5dg5f7f76gtf5dr5erdftfgvbnhju", Timestamp = 1234},
                new RegistrationBody() { UserHandle = "austin powers", PublicKey = "ghf5dg5f7f76gtr5dr5erdftfgtdwhju", Timestamp = 4321},
        }
        };
        [TestMethod()]
        public void GetPayloadSizeTest()
        {
            Assert.AreEqual(rb.GetPayloadSize(), (uint)(rb.Registrations.ToArray()[0].GetPayloadSize() + rb.Registrations.ToArray()[1].GetPayloadSize() + 4));
        }

        [TestMethod()]
        public void SerializeTest()
        {
            byte[] data = rb.Serialize();
            RegistrationsBody rb2 = new RegistrationsBody();
            rb2.Inflate(new MemoryStream(data));
            Assert.IsTrue(rb.Registrations.ToArray()[0].Equals(rb2.Registrations.ToArray()[0]));
            Assert.IsTrue(rb.Registrations.ToArray()[1].Equals(rb2.Registrations.ToArray()[1]));
        }
    }
}
