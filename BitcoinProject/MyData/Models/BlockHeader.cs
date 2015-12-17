using System;
using System.IO;
using System.Threading;
using Util;

namespace Models
{
	/**
	 * @see https://bitcoin.org/en/developer-reference#block-headers
	 **/
	public class BlockHeader : SerializableToBinary
	{

		public uint Version { get; set;}
		public string PreviousBlockHeaderHash { get; set; }
		public string MerkleRootHash { get; set; }
		public uint Timer { get; set; }
		public uint NBits { get; set; }
		public uint Nonce { get; set; }

		public BlockHeader (Stream input)
		{
			Version = BinaryUtil.UintFromStream (input, 4);

			PreviousBlockHeaderHash = BinaryUtil.StringFromStream (input, 32);

			MerkleRootHash = BinaryUtil.StringFromStream (input, 32);

			Timer = BinaryUtil.UintFromStream (input, 4);

			NBits = BinaryUtil.UintFromStream (input, 4);

			Nonce = BinaryUtil.UintFromStream (input, 4);
		}

		#region SerializableToBinary implementation

		public byte[] Serialize ()
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

