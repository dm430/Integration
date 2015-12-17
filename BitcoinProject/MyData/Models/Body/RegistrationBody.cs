using System;
using System.Text;
using System.IO;
using Util;
using Microsoft.CSharp.RuntimeBinder;

namespace Models.Body
{
    /**
	 * Sent when a registration is broadcast
	 * accross the network, or when getreg
	 * is received.
	 **/
    [BodyCommand("reg")]
    public class RegistrationBody : Body
    {
        /**
		 * The username that is being registered
		 **/
        public string UserHandle { get; set; }
        /**
		 * The public key of the user registering
		 **/
        public string PublicKey { get; set; }

        /**
		 * The timestamp of when the user was first
		 * registered.
		 **/
        public long Timestamp { get; set; }

        public override uint GetPayloadSize()
        {
            return (uint)(UserHandle.Length + PublicKey.Length + 12);
        }

        public override void Inflate(Stream input)
        {
            int length = BinaryUtil.IntFromStream(input, 4);
            UserHandle = BinaryUtil.StringFromStream(input, length);
            PublicKey = BinaryUtil.StringFromStream(input, 32);
            Timestamp = BinaryUtil.LongFromStream(input, 8);
        }

        public override byte[] Serialize()
        {
            byte[] output = new byte[UserHandle.Length + 4 + 32 + 8];
            Buffer.BlockCopy(new[] { UserHandle.Length }, 0, output, 0, 4);
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(UserHandle), 0, output, 4, UserHandle.Length);
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(PublicKey), 0, output, 4 + UserHandle.Length, PublicKey.Length);
            Buffer.BlockCopy(new long[] { Timestamp }, 0, output, 4 + UserHandle.Length + PublicKey.Length, 8);
            return output;
        }

        public override bool Equals(object obj)
        {
            if(obj is RegistrationBody)
            {
                RegistrationBody rb = (RegistrationBody)obj;
                return this.PublicKey == rb.PublicKey &&
                    this.UserHandle == rb.UserHandle &&
                    this.Timestamp == rb.Timestamp;
            }
            return false;
        }
    }
}

