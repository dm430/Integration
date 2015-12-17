using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Models.Body
{
	/**
	 * Asks a peer to compare message
	 * hashes to get all messages that
	 * may have not been received.
	 * 
	 * The peer should send a messsages
	 * object in response, even if sending
	 * 0 messages.
	 **/
	[BodyCommand("cmprhash")]
    public class CompareHashesBody : Body
    {

        public IEnumerable<string> Hashes { get; set; }

        #region implemented abstract members of Body

        public override uint GetPayloadSize()
        {
            return (uint)(Hashes.Count() * 32) + 4;
        }

        public override void Inflate(System.IO.Stream input)
        {
            int count = BinaryUtil.IntFromStream(input, 4);
            List<string> hashes = new List<string>();
            while(count > 0)
            {
                hashes.Add(BinaryUtil.StringFromStream(input, 32));
                count--;
            }
            Hashes = hashes;
        }

        public override byte[] Serialize()
        {
            byte[] data = new byte[GetPayloadSize()];
            Buffer.BlockCopy(new[] { Hashes.Count() }, 0, data, 0, 4);
            int index = 4;
            foreach (string hash in Hashes)
            {
                Buffer.BlockCopy(BinaryUtil.Serialize(hash, hash.Length), 0, data, index, hash.Length);
                index += hash.Length;
            }
            return data;
        }

        #endregion
    }
}

