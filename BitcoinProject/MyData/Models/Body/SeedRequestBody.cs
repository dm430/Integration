using System;
using System.IO;
using Util;
using System.Collections.Generic;

namespace Models.Body
{
	/**
	 * Used to ask the seed server for a
	 * list if IP address that belong to
	 * network peers.
	 **/
	[BodyCommand("seedreq")]
	public class SeedRequestBody : Body
	{
		public uint IPCount { get; set; }

        #region implemented abstract members of Body

        public override uint GetPayloadSize()
        {
            return 4;
        }

        public override void Inflate (Stream input)
		{
			IPCount = BinaryUtil.UintFromStream (input, 4);
		}

		public override byte[] Serialize ()
		{
			List<Byte> output = new List<Byte> ();
			output.AddRange (BinaryUtil.Serialize(IPCount));
			return output.ToArray ();
		}
		#endregion
	}
}

