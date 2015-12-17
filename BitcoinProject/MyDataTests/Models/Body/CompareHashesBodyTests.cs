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
    public class CompareHashesBodyTests
    {


        [TestMethod()]
        public void GetPayloadSizeTest()
        {
            CompareHashesBody CHB = new CompareHashesBody() { Hashes = new List<string> { "QWERQWERQWERQWERQWERQWERQWERQWER" } };
            uint payloadSize = CHB.GetPayloadSize();
            uint payloadSizeSize = 32;
            uint completeSize = (uint)CHB.Hashes.Count() + payloadSizeSize;
            uint stuff = (uint)CHB.Hashes.ToArray()[0].Length;
            Assert.AreEqual(payloadSize, (stuff + 4));

        }

        [TestMethod()]
        public void SerializeTest()
        {
            CompareHashesBody CHB = new CompareHashesBody() { Hashes = new List<string> { "QWERQWERQWERQWERQWERQWERQWERQWER" } };
            byte[] bytes = CHB.Serialize();
            Stream stream = new MemoryStream(bytes);
            CompareHashesBody CHB2 = new CompareHashesBody();
            CHB2.Inflate(stream);
            Assert.AreEqual(CHB.Hashes.ElementAt(0), CHB2.Hashes.ElementAt(0));
        }
    }
}
