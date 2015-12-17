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
    public class SeedRequestBodyTests
    {
        private uint IpCount = 7;

        [TestMethod()]
        public void GetPayloadSizeTest()
        {
            SeedRequestBody SRB = new SeedRequestBody() { IPCount = IpCount };
            uint size = SRB.GetPayloadSize();
            Assert.AreEqual(size, (uint)4);
        }
        [TestMethod()]
        public void SerializeTest()
        {
            SeedRequestBody SRB = new SeedRequestBody() { IPCount = IpCount };
            byte[] bytes = SRB.Serialize();
            Stream stream = new MemoryStream(bytes);
            SeedRequestBody SRB2 = new SeedRequestBody();
            SRB2.Inflate(stream);
            Assert.AreEqual(IpCount, SRB2.IPCount);
        }

        
    }
}
