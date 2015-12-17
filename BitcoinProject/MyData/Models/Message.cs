using System;
using System.IO;
using System.Collections.Generic;
using Util;
using System.Net.Sockets;
using System.Text;

namespace Models
{
	/**
	 * This is the class that you serialize and
	 * send accross the wire (through the socket).
	 * Make sure you initialize all of the
	 * properties to the correct values, otherwise
	 * you will get exceptions. Although for your
	 * convenience, I have filled out some of the
	 * properties in the consructor for you.
	 * You're welcome.
	 **/
	public class Message : SerializableToBinary
	{
		/**
		 * Don't worry about filling this in
		 * with anything useful. Just use
		 * NetworkMode.MAIN
		 **/
		public NetworkMode Mode { get; set; }

		/**
		 * Make sure this matches what you're
		 * expecting in MessageHandler
		 **/
		public string CommandName { get; set; }

		/**
		 * You can implement this if you want.
		 * Or just not.
		 * Who needs uncorrupted messages?
		 **/
		public string Checksum { get; set; }

		/**
		 * This is the meat of the message.
		 * Make sure you initialize the proper
		 * subclass of Body and fill in ALL of
		 * its properties.
		 **/
		public Body.Body Body { get; set; }

		/**
		 * The constructor to use if you are
		 * constructing a message manually.
		 * Fill in ALL of the properties
		 * except those automatically assigned
		 * here in this constructor.
		 * 
		 * The automatic ones include:
		 * - Mode
		 * - Checksum (not an actual checksum)
		 * - Hash
		 **/
		public Message ()
        {
            Mode = NetworkMode.MAIN;
            Checksum = "\0\0\0\0";
        }

		public Message (NetworkMode currentMode, Stream input)
		{

            // Check the magic string
            byte[] magic = new byte[4];
			input.Read(magic, 0, magic.Length);
			Mode = NetworkMode.fromMagic (magic);

			if(Mode == null){
				throw new MisplacedCursorException ();
			}


			// 12 bytes command name - char[12]
			byte[] command = new byte[12];
			input.Read (command, 0, command.Length);
			CommandName = System.Text.Encoding.Default.GetString (command);
            CommandName = CommandName.Replace("\0", "");
            // 4 bytes payload size - uint32_t
            // Don't know if we should use this or not.
            // The Message Body 
            byte[] payloadSize = new byte[4];
			input.Read (payloadSize, 0, payloadSize.Length);
			/* uint bodySize = */ BitConverter.ToUInt32 (payloadSize, 0);

			// 4 bytes checksum - char[4]
			byte[] checksum = new byte[4];
			input.Read (checksum, 0, checksum.Length); 
			// TODO - Don't ignore checksum

			// Grab body and inflate
			Body = Models.Body.Body.getBody(CommandName, input);
		}

		#region SerializableToBinary implementation

		public byte[] Serialize ()
		{
			List<Byte> output = new List<Byte> ();
			output.AddRange (Mode.MagicBytes);
			output.AddRange (BinaryUtil.Serialize (CommandName, 12));
			output.AddRange (BinaryUtil.Serialize (Body.GetPayloadSize()));
			output.AddRange (BinaryUtil.Serialize (0));
			output.AddRange (Body.Serialize ()); 
			return output.ToArray ();
		}

		#endregion
	}

	/**
	 * I had high hopes for this. Poor
	 * bitcoin never had a chance. If
	 * only it were a quarter-long course...
	 **/
	public class NetworkMode{
		public static readonly NetworkMode MAIN = new NetworkMode (8333, new byte[]{0xf9, 0xbe, 0xb4, 0xd9});
		public static readonly NetworkMode TEST = new NetworkMode (18333, new byte[]{0x0b, 0x11, 0x09, 0x07});
		public static readonly NetworkMode REGTEST = new NetworkMode (18444, new byte[]{0xfa, 0xbf, 0xb5, 0xda});

		static List<NetworkMode> values { get; set; } 

		public int Port { get; private set; }
		public byte[] MagicBytes { get; private set; }

		static NetworkMode(){
            values = new List<NetworkMode>();
			values.AddRange (new NetworkMode[]{MAIN, TEST, REGTEST});
		}

		NetworkMode (int port, byte[] bytes)
		{
			Port = port;
			MagicBytes = bytes;
		}

		public static NetworkMode fromMagic(byte[] magic){
            string magicString = Encoding.ASCII.GetString(magic);
                foreach(NetworkMode mode in values){
                if (magicString == Encoding.ASCII.GetString(mode.MagicBytes)){
					return mode;
				}
			}
			return null;
		}
	}
}

