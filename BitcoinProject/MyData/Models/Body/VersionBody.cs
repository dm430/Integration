using System;
using System.IO;
using Util;
using System.Linq;
using System.Text;

namespace Models.Body
{
	/**
	 * First message ever sent to new peers. It is 
	 * required to establish a connection. The
	 * information contained in this message is basically
	 * useless at this point, but you can use it if you'd
	 * like.
	 **/
	[BodyCommand("version")]
	public class VersionBody : Body
	{
		//		[ 4 bytes ]
		public int Version { get; set; }
		// This will contain multiple services stored
		// as bits.
		//		[ 8 bytes ]
		public Service Services { get; set; }

		public long Timestamp { get; set; }

		/**		[ 8 bytes ]
		 * The services supported by the receiving
		 * node as perceived by the transmitting node.
		 * Same format as the ‘services’ field above.
		 * Bitcoin Core will attempt to provide accurate information.
		 * BitcoinJ will, by default, always send 0.
		 **/
		public Service AddrRecvServices { get; set; }

		/**		[ 16 bytes ]
		 * The IPv6 address of the receiving node as 
		 * perceived by the transmitting node in big 
		 * endian byte order. IPv4 addresses can be 
		 * provided as IPv4-mapped IPv6 addresses. 
		 * Bitcoin Core will attempt to provide accurate 
		 * information. BitcoinJ will, by default, always 
		 * return ::ffff:127.0.0.1
		 **/
		public string AddrRecvIPAddress { get; set; }

		//		[ 2 bytes ]
		public short AddrRecvPort { get; set; }

		// Should be identical to above. Doesn't need to be repeated.
		// public Service AddrTransServices { get; set; }

		/**		[ 16 bytes ]
		 * The IPv6 address of the transmitting node in
		 * big endian byte order. IPv4 addresses can be 
		 * provided as IPv4-mapped IPv6 addresses. Set 
		 * to ::ffff:127.0.0.1 if unknown.
		 **/
		public string AddrTransIpAddress { get; set; }

		//		[ 2 bytes ]
		public short AddrTransPort { get; set; }

		/**		[ 8 bytes ]
		 * A random nonce which can help a node detect 
		 * a connection to itself. If the nonce is 0, 
		 * the nonce field is ignored. If the nonce is 
		 * anything else, a node should terminate the 
		 * connection on receipt of a version message 
		 * with a nonce it previously sent.
		 **/
		public ulong Nonce { get; set; }

		//		[ Varies ]
		// public uint UserAgentSize { get; set; }

		/**		[ Varies ]
		 * User agent as defined by BIP14. Previously 
		 * called subVer.
		 * 
		 * @see https://github.com/bitcoin/bips/blob/master/bip-0014.mediawiki
		 **/
		public String UserAgent { get; set; }

		/**		[ 4 bytes ]
		 * The height of the transmitting node’s best
		 * block chain or, in the case of an SPV client, 
		 * best block header chain.
		 **/
		public int StartHeight { get; set; }

		/**		[ 1 byte ]
		 * Transaction relay flag. If 0x00, no inv 
		 * messages or tx messages announcing new 
		 * transactions should be sent to this client 
		 * until it sends a filterload message or 
		 * filterclear message. If 0x01, this node 
		 * wants inv messages and tx messages 
		 * announcing new transactions.
		 **/
		public bool Relay { get; set; }

		public VersionBody(){
			Version = 70012;
			Services = Service.NODE_NETWORK;
			Timestamp = DateTime.Now.ToBinary ();
			AddrRecvServices = Service.NODE_NETWORK;
			AddrTransIpAddress = "::ffff:127.0.0.1";
			AddrTransPort = (short)Constants.DEFAULT_NETWORK_PORT;
			UserAgent = "neubitcoin";
			Relay = true;
		}

		#region implemented abstract members of Body

		public override uint GetPayloadSize ()
		{
			return (uint)Serialize ().Count ();
		}

		public override void Inflate (Stream input)
		{
			return;
			/*
			Version = BinaryUtil.IntFromStream (input, 4);
			Services = (Service)BinaryUtil.UlongFromStream (input, 8);
			Timestamp = BinaryUtil.LongFromStream (input, 8);
			AddrRecvServices = (Service)BinaryUtil.UlongFromStream (input, 8);
			AddrRecvIPAddress = BinaryUtil.StringFromStream (input, 16);
			AddrRecvPort = BinaryUtil.ShortFromStream (input, 2);
			// AddrTransServices = long
			BinaryUtil.UlongFromStream (input, 8);
			AddrTransIpAddress = BinaryUtil.StringFromStream (input, 16);
			AddrTransPort = BinaryUtil.ShortFromStream (input, 2);
			Nonce = BinaryUtil.UlongFromStream (input, 8);

			UInt64 size = BinaryUtil.CompactUlongFromStream (input);
			UserAgent = BinaryUtil.StringFromStream (input, (long)size);
			StartHeight = BinaryUtil.IntFromStream (input, 4);
            Relay = BinaryUtil.BoolFromStream(input);
			*/
		}

		public override byte[] Serialize ()
		{
			byte[] UserAgentSize = BinaryUtil.LongToCompactBytes ((ulong)UserAgent.LongCount ());

			byte[] output = new byte[4+8+8+8+AddrRecvIPAddress.Length+2+8+AddrTransIpAddress.Length+2+8+UserAgentSize.Length+UserAgent.Length+4+1]; //[1 + 4 + UserAgent.LongCount () + UserAgentSize.Length + 8 + 2 + 16 + 8 + 2 + 16 + 8 + 8 + 8 + 4];

			int start = 0;

            Buffer.BlockCopy(new[] { Version }, 0, output, 0, 4);
            Buffer.BlockCopy(new[] { (long)Services }, 0, output, (start += 4), 8);
            Buffer.BlockCopy(new[] { Timestamp }, 0, output, (start += 8), 8);
            Buffer.BlockCopy(new[] { (long)AddrRecvServices }, 0, output, (start += 8), 8);
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(AddrRecvIPAddress), 0, output, (start += 8), AddrRecvIPAddress.Length);
            Buffer.BlockCopy(new[] { AddrRecvPort }, 0, output, (start += AddrRecvIPAddress.Length), 2);
            start += 2; // Skipping AddrTransServices (8 bytes)
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(AddrTransIpAddress), 0, output, (start += 8), AddrTransIpAddress.Length);
            Buffer.BlockCopy(new[] { AddrTransPort }, 0, output, (start += AddrTransIpAddress.Length), 2);
            Buffer.BlockCopy(new[] { Nonce }, 0, output, (start += 2), 8);
            Buffer.BlockCopy(UserAgentSize, 0, output, (start += 8), UserAgentSize.Length);
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(UserAgent), 0, output, (start += UserAgentSize.Length), UserAgent.Length); // 6-?
            Buffer.BlockCopy(new[] { StartHeight }, 0, output, (start += UserAgent.Length), 4); // 1-5
            output[start + 4] = (byte)(Relay ? 1 : 0); // 0


			return new byte[]{};
		}

		#endregion
	}

	/**
	 * Well this is pretty lame. The docs
	 * advertise Services as pieces that
	 * your client can implement that others
	 * may want to utilize, but there is only
	 * Unnamed (basic node) and NODE_NETWORK
	 * whic is the full network node. You're
	 * supposed to typically OR all of the services
	 * you want to use together, but there's only
	 * one, so just always set it to this I
	 * guess... *sigh*
	 **/
	public enum Service : long {
		// Unnamed = 0x00
		NODE_NETWORK = 0X01
	}
}

