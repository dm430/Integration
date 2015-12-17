using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Body;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Util;
namespace Models.Body.Tests
{
    [TestClass()]
    public class VersionBodyTests
    {

        [TestMethod()]
        public void GetPayloadSizeTest()
        {
            VersionBody VB = new VersionBody();
            VB.AddrRecvIPAddress = "::ffff:127.0.0.9";
            VB.AddrRecvPort = 119;
            VB.Nonce = 119;
            VB.StartHeight = 119;

            VB.Version = 70013;
            VB.Services = Service.NODE_NETWORK;
            VB.Timestamp = DateTime.Now.ToBinary();
            VB.AddrRecvServices = Service.NODE_NETWORK;
            VB.AddrTransIpAddress = "::ffff:127.0.0.7";
            VB.AddrTransPort = (short)Constants.DEFAULT_NETWORK_PORT;
            VB.UserAgent = "noobitcoin";
            VB.Relay = false;
            uint size = VB.GetPayloadSize();
            uint expected = (uint)(4 + 8 + 8 + 8 + VB.AddrRecvIPAddress.Length + 2 + 8 + VB.AddrTransIpAddress.Length + 2 + 8 + 8 + VB.UserAgent.Length + 4 + 1);
            Assert.AreEqual(size, expected);
        }

        [TestMethod()]
        public void SerializeAndInflateTest()
        {
            VersionBody VB = new VersionBody();
            VB.AddrRecvIPAddress = "::ffff:127.0.0.9";
            VB.AddrRecvPort = 119;
            VB.Nonce = 119;
            VB.StartHeight = 119;

            VB.Version = 70013;
			VB.Services = Service.NODE_NETWORK;
			VB.Timestamp = DateTime.Now.ToBinary ();
			VB.AddrRecvServices = Service.NODE_NETWORK;
			VB.AddrTransIpAddress = "::ffff:127.0.0.7";
			VB.AddrTransPort = (short)Constants.DEFAULT_NETWORK_PORT;
			VB.UserAgent = "noobitcoin";
			VB.Relay = false;

            byte[] bytes = VB.Serialize();
            Stream s = new MemoryStream(bytes);

            VersionBody VB2 = new VersionBody();
            VB2.Inflate(s);

            Assert.AreEqual(VB.Version, VB2.Version);
            Assert.AreEqual(VB.Services, VB2.Services);
            Assert.AreEqual(VB.Timestamp, VB2.Timestamp);
            Assert.AreEqual(VB.AddrRecvServices, VB2.AddrRecvServices);
            Assert.AreEqual(VB.AddrRecvIPAddress, VB2.AddrRecvIPAddress);
            Assert.AreEqual(VB.AddrRecvPort, VB2.AddrRecvPort);
            Assert.AreEqual(VB.AddrTransIpAddress, VB2.AddrTransIpAddress);
            Assert.AreEqual(VB.AddrTransPort, VB2.AddrTransPort);
            Assert.AreEqual(VB.Nonce, VB2.Nonce);
            Assert.AreEqual(VB.UserAgent, VB2.UserAgent);
            Assert.AreEqual(VB.StartHeight, VB2.StartHeight);
            Assert.AreEqual(VB.Relay, VB2.Relay);
        }
    }
}
