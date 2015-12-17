using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Body;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyData.Models;
using System.IO;

namespace Models.Body.Tests
{
    [TestClass()]
    public class MessagesBodyTests
    {
        List<TextMessage> messages = new List<TextMessage>()
        {
            new TextMessage() { UserHandle ="darthvader", Text = "i am your father", Hash = "hjgtfrdhtyghtyfresdefgtftg56rdft" },
            new TextMessage() { UserHandle = "superman", Text = "faster than a speeding bullet", Hash = "h3hdj5hd7s8d6fhenc7dhnju865rtfds" }
        };

        [TestMethod()]
        public void GetPayloadSizeTest()
        {
            MessagesBody mb = new MessagesBody() { Messages = messages };
            Assert.AreEqual(mb.GetPayloadSize(), (messages[0].GetPayloadSize() + messages[1].GetPayloadSize() + 4));
        }

        [TestMethod()]
        public void SerializeTest()
        {
            MessagesBody mb = new MessagesBody() { Messages = messages };
            byte[] data = mb.Serialize();
            MessagesBody newmb = new MessagesBody();
            newmb.Inflate(new MemoryStream(data));
            for (int i = 0; i < messages.Count; i++)
            {
                Assert.IsTrue(mb.Messages.ToArray()[i].Equals(newmb.Messages.ToArray()[i]));
            }
        }
    }
}
