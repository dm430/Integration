using System;
using System.IO;
using System.Text;
using System.Linq;

namespace Util
{
	public static class BinaryUtil
	{
		public static uint UintFromStream(Stream input, short bytes){
			var data = new byte[bytes];
			input.Read (data, 0, data.Length);
            data = data.Reverse().ToArray();
			uint value = BitConverter.ToUInt32 (data, 0);
			return value;
		}

		public static int IntFromStream (Stream input, short bytes)
		{
			var data = new byte[bytes];
			input.Read (data, 0, data.Length);
			int value = BitConverter.ToInt32 (data, 0);
			return value;
		}

		public static ulong UlongFromStream(Stream input, short bytes){
			var data = new byte[bytes];
			input.Read (data, 0, data.Length);
			ulong value = BitConverter.ToUInt64 (data, 0);
			return value;
		}

		public static long LongFromStream (Stream input, short bytes)
		{
			var data = new byte[bytes];
			input.Read (data, 0, data.Length);
			long value = BitConverter.ToInt64 (data, 0);
			return value;
		}

		public static short ShortFromStream (Stream input, short bytes)
		{
			var data = new byte[bytes];
			input.Read (data, 0, data.Length);
			short value = BitConverter.ToInt16 (data, 0);
			return value;
		}
   
        public static bool BoolFromStream(Stream input)
        {
            var data = new byte[1];
            input.Read(data, 0, data.Length);
            return BitConverter.ToBoolean(data, 0);
        }

        public static byte[] Read(Stream input, int bytes){
			if(bytes < 0){
				throw new ArgumentException ("bytes cannot be negative.");
			}
			return Read (input, (ulong)bytes);
		}

		public static byte[] Read(Stream input, ulong bytes){
			var data = new byte[bytes];
			input.Read (data, 0, data.Length);
			return data;
		}

		public static string StringFromStream(Stream input, long bytes){
			var data = new byte[bytes];
			input.Read (data, 0, data.Length);
			string output = System.Text.Encoding.ASCII.GetString (data);
			return output;
		}

		/**
		 * @see https://bitcoin.org/en/developer-reference#compactsize-unsigned-integers
		 * */
		public static ulong CompactUlongFromStream(Stream input){
			ulong output = 0;
			byte test = (byte) input.ReadByte ();

			if(test <= 252) {
				output = test;
			} else if(test == 0xFD){
				output = UlongFromStream (input, 2);
			} else if(test == 0xFE){
				output = UlongFromStream (input, 4);
			} else if(test == 0xFF){
				output = UlongFromStream (input, 8);
			}

			return output;
		}

		public static byte[] LongToCompactBytes(ulong input){
			byte[] output;
			if(input <= 252){
				output = new byte[1];
				output [0] = (byte)input;
			} else if (input <= (ulong)short.MaxValue){
				output = new byte[3];
				output [0] = 0xFD;
				output [1] = (byte)(input >> 8);
				output [2] = (byte)(input);
			} else if (input <= int.MaxValue){
				output = new byte[4];
				output [0] = 0xFE;
				output [1] = (byte)(input >> (8 * 3));
				output [2] = (byte)(input >> (8 * 2));
				output [3] = (byte)(input >> (8 * 1));
				output [4] = (byte)(input >> (8 * 0));
			} else {
				output = new byte[9];
				output [0] = 0xFF;
				output [1] = (byte)(input >> (8 * 7));
				output [2] = (byte)(input >> (8 * 6));
				output [3] = (byte)(input >> (8 * 5));
				output [4] = (byte)(input >> (8 * 4));
				output [5] = (byte)(input >> (8 * 3));
				output [6] = (byte)(input >> (8 * 2));
				output [7] = (byte)(input >> (8 * 1));
				output [8] = (byte)(input >> (8 * 0));
			}
			return output;
		}

		public static byte[] Serialize (uint x)
		{
			var src = new uint[]{ x };
			var dst = new byte[4];
			Buffer.BlockCopy (src, 0, dst, 0, 4);
			return dst.Reverse().ToArray ();
		}

        public static byte[] Serialize(int x)
        {
            var src = new int[] { x };
            var dst = new byte[4];
            Buffer.BlockCopy(src, 0, dst, 0, 4);
            return dst.Reverse().ToArray();
        }

        public static byte[] Serialize(ulong x)
        {
            var src = new ulong[] { x };
            var dst = new byte[8];
            Buffer.BlockCopy(src, 0, dst, 0, 8);
            return dst.Reverse().ToArray();
        }

        public static byte[] Serialize(long x)
        {
            var src = new long[] { x };
            var dst = new byte[8];
            Buffer.BlockCopy(src, 0, dst, 0, 8);
            return dst.Reverse().ToArray();
        }

        public static byte[] Serialize (string input, int len)
		{
            var dst = new byte[len];
			Buffer.BlockCopy (Encoding.ASCII.GetBytes(input), 0, dst, 0, input.Length);
			return dst;
		}

        public static byte[] Serialize(string input, uint len)
        {
            var dst = new byte[len];
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(input), 0, dst, 0, input.Length);
            return dst;
        }

        public static byte[] Serialize(string input, long len)
        {
            var dst = new byte[len];
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(input), 0, dst, 0, input.Length);
            return dst;
        }

        public static byte[] Serialize(string input, ulong len)
        {
            var dst = new byte[len];
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(input), 0, dst, 0, input.Length);
            return dst;
        }
    }
}

