using System;
using System.IO;
using Util;
using System.Linq;

namespace Models.Body
{
	/**
	 * Sent in response to seedreq by seed
	 * server. Contains a list of IP addresses
	 * that belong to network peers.
	 **/
	[BodyCommand("seedresp")]
	public class SeedResponseBody : Body
	{
		public byte[,] IpAddresses { get; set; }

        #region implemented abstract members of Body

        public override uint GetPayloadSize()
        {
            return (uint)IpAddresses.Length;
        }

        public override void Inflate (Stream input)
		{
			uint count = BinaryUtil.UintFromStream (input, 4);
			IpAddresses = new byte[count, 4];
			for(int i = 0; i < count ; i++){
				
				byte[] ip = new byte[4];
				input.Read (ip, 0, 4);

				// Ugh C# won't let you copy the reference to a 2d array
				for(int j = 0; j < ip.Length; j++)
					IpAddresses [i, j] = ip[j];
				
			}
		}

		public override byte[] Serialize ()
		{
            if(IpAddresses.Length == 0)
            {
                return new byte[4];
            }
			byte[] output = new byte[IpAddresses.Length + 4];

			byte[] count = BitConverter.GetBytes (IpAddresses.Length / 4);
            count = count.Reverse().ToArray();
            for (int i = 0; i < count.Length; i++)
            {
                output[i] = count[i];
            }

            //Buffer.BlockCopy (output, 0, count, 0, 4);

            //output = output.Reverse ().ToArray ();

			for(int i = 0; i < IpAddresses.Length/4; i++){
				for(int j = 0; j < 4; j++){
					output [(i * 4) + j + 4] = IpAddresses [i, j];
				}
			}
			return output;
		}
		#endregion
	}
}
