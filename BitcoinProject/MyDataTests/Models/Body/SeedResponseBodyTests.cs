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
    public class SeedResponseBodyTests
    {
        byte[,] Input = new byte[,]{ {192, 168, 1, 119}, {192, 168, 1, 100}};
        
        [TestMethod()]
        public void SerializeTest()
        {
            SeedResponseBody SRB = new SeedResponseBody() { IpAddresses = Input };
            byte[] stuff = SRB.Serialize();
        }

        [TestMethod()]
        public void SerializeAndInflateTest()
        {
            SeedResponseBody SRB = new SeedResponseBody() { IpAddresses = Input };
            byte[] stuff = SRB.Serialize();
            Stream stream = new MemoryStream(stuff);
            SeedResponseBody SRB2 = new SeedResponseBody();
            SRB2.Inflate(stream);
            for (int i = 0; i < Input.GetLength(0); i++)
            {
                for (int i2 = 0; i2 < Input.GetLength(1); i2++)
                {
                    Assert.AreEqual(Input[i, i2], SRB2.IpAddresses[i, i2]);
                }
            }
        }

        [TestMethod()]
        public void GetPayloadSizeTest()
        {
            SeedResponseBody SRB = new SeedResponseBody() { IpAddresses = Input };
            uint size = SRB.GetPayloadSize();
            Assert.AreEqual(size, (uint)8);
        }
    }
}
