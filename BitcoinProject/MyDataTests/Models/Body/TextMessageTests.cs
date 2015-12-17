using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyData.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Models.Body;

namespace MyData.Models.Tests
{
    [TestClass()]
    public class TextMessageTests
    {
        private string TestUserHandle = "#YoloSwag";
        private string TestHash = "b2fdc0d3bd1ff38d458f6b1210e47e62";
        private string TestText = "This is how we write unit tests.... After midnight.";

        [TestMethod()]
        public void GetPayloadSizeTest()
        {
            TextMessage TM = new TextMessage() { UserHandle = TestUserHandle, Hash = TestHash, Text = TestText };
            uint payloadSize = TM.GetPayloadSize();
            Assert.AreEqual(payloadSize, (uint)(4 + TestUserHandle.Length + 4 + TestHash.Length + TestText.Length + 8));
        }

        [TestMethod()]
        public void SerializeTest()
        {
            TextMessage TM = new TextMessage() { UserHandle = TestUserHandle, Hash = TestHash, Text = TestText };
            byte[] bytes = TM.Serialize();
            Stream stream = new MemoryStream(bytes);
            TextMessage TM2 = new TextMessage();
            TM2.Inflate(stream);
            Assert.AreEqual(TestUserHandle, TM2.UserHandle);
            Assert.AreEqual(TestHash, TM2.Hash);
            Assert.AreEqual(TestText, TM2.Text);
        }
    }
}
