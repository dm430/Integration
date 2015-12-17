using System;
using Util;
using System.Linq;
using System.Text;

namespace Models.Body
{
	/**
	 * Asks a peer for registration info.
	 * Useful for checking facts.
	 * 
	 * Expects a response of reg
	 **/
	[BodyCommand("getreg")]
	public class GetRegistrationBody : Body
    {
        /**		[ varies ]
		 * A user name?
		 **/
        public String UserHandle { get; set; }
		public GetRegistrationBody ()
		{
		}

		#region implemented abstract members of Body

		public override uint GetPayloadSize ()
        {
            return (uint)Serialize().Length;
		}

		public override void Inflate (System.IO.Stream input)
        {
            int length = BinaryUtil.IntFromStream(input, 4);
            UserHandle = BinaryUtil.StringFromStream(input, length);
		}

		public override byte[] Serialize ()
        {
            byte[] output = new byte[UserHandle.Length + 4];
            Buffer.BlockCopy(new []{ UserHandle.Length }, 0, output, 0, 4);
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(UserHandle), 0, output, 4, UserHandle.Length);
            return output;
		}

		#endregion
	}
}

