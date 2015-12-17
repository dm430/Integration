using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Util;

namespace Models.Body
{
	/**
	 * Contains a list of registrations that
	 * is sent in response to getregs
	 **/
	[BodyCommand("regs")]
	public class RegistrationsBody : Body
	{

		public IEnumerable<RegistrationBody> Registrations { get; set; }

		#region implemented abstract members of Body

		public override uint GetPayloadSize ()
		{
            uint count = 4;
            foreach(RegistrationBody rb in Registrations)
            {
                count += rb.GetPayloadSize();
            }
            return count;
		}

		public override void Inflate (Stream input)
		{
            int count = BinaryUtil.IntFromStream(input, 4);
            List<RegistrationBody> registrations = new List<RegistrationBody> ();
			while(count > 0)
            {
                RegistrationBody rb = new RegistrationBody();
                rb.Inflate(input);
                registrations.Add(rb);
                count--;
            }
            Registrations = registrations;
		}

		public override byte[] Serialize ()
		{
            byte[] data = new byte[GetPayloadSize()];
			Buffer.BlockCopy (new []{Registrations.Count()}, 0, data, 0, 4);
            int index = 4;
            foreach(RegistrationBody rb in Registrations)
            {
                Buffer.BlockCopy(rb.Serialize(), 0, data, index, (int)rb.GetPayloadSize());
                index += (int)rb.GetPayloadSize();
            }
            return data;
		}

		#endregion
	}
}

