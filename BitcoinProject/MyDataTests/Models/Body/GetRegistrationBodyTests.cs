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
    public class GetRegistrationBodyTests
    {
        private string TestString = "#YoloSwag";

        [TestMethod()]
        public void GetPayloadSizeTest()
        {
            GetRegistrationBody GRB = new GetRegistrationBody() { UserHandle = TestString };
            uint payloadSize = GRB.GetPayloadSize();
            uint payloadSizeSize = 4;
            uint completeSize = (uint)TestString.Length + payloadSizeSize;
            Assert.AreEqual(payloadSize, completeSize);
            
        }

        [TestMethod()]
        public void SerializeTest()
        {
            GetRegistrationBody GRB = new GetRegistrationBody() { UserHandle = TestString };
            byte[] bytes = GRB.Serialize();
            Stream stream = new MemoryStream(bytes);
            GetRegistrationBody GRB2 = new GetRegistrationBody();
            GRB2.Inflate(stream);
            Assert.AreEqual(TestString, GRB2.UserHandle);
        }
    }
}