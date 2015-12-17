using Models.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using System.Security.Policy;

namespace Models.Body
{
	/**
	 * A text message sent by a user
	 * to all other users on the network
	 **/
	[BodyCommand("txtmsg")]
    public class TextMessage : Body
    {

        public string UserHandle { get; set; }
        
        //Bytes[]
        public string Text { get; set; }

        //Bytes[]
        public string Hash { get; set; }

		public long Timestamp { get; set; }

        public override uint GetPayloadSize()
        {
            return (uint)(UserHandle.Length + Text.Length + 48);
        }

        public override void Inflate(System.IO.Stream input)
        {
            int UserHandleLength = BinaryUtil.IntFromStream(input, 4);
            UserHandle = BinaryUtil.StringFromStream(input, UserHandleLength);
            int textLength = BinaryUtil.IntFromStream(input, 4);
            Text = BinaryUtil.StringFromStream(input, textLength);
            Hash = BinaryUtil.StringFromStream(input, 32);
			Timestamp = BinaryUtil.LongFromStream (input, 8);
        }

        public override byte[] Serialize()
        {
            byte[] data = new byte[UserHandle.Length + Text.Length + 48];
            Buffer.BlockCopy(new[] { UserHandle.Length }, 0, data, 0, 4);
            Buffer.BlockCopy(BinaryUtil.Serialize(UserHandle, UserHandle.Length), 0, data, 4, UserHandle.Length);
            Buffer.BlockCopy(new [] {Text.Length}, 0, data, UserHandle.Length + 4, 4);
            Buffer.BlockCopy(BinaryUtil.Serialize(Text, Text.Length), 0, data, UserHandle.Length + 8, Text.Length);
            Buffer.BlockCopy(BinaryUtil.Serialize(Hash, Hash.Length), 0, data, UserHandle.Length + Text.Length + 8, Hash.Length);
			Buffer.BlockCopy (BinaryUtil.Serialize (Timestamp), 0, data, UserHandle.Length + Text.Length + 8 + Hash.Length, 8);
            return data;
        }


        public override bool Equals(object obj)
        {
            if(obj is TextMessage)
            {
                TextMessage txt = (TextMessage)obj;
				return this.Hash == txt.Hash &&
				this.Text == txt.Text &&
				this.UserHandle == txt.UserHandle &&
				this.Timestamp == txt.Timestamp;
            }
            return false;
        }
    }
}
