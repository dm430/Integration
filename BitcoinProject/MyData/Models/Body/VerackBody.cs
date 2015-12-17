using System;

namespace Models.Body
{
	/**
	 * Sent in response to a version message.
	 * Although it may seem empty, it is necessary
	 * to include this in the message sent
	 * as a reponse to the version message.
	 **/
	[BodyCommand("verack")]
	public class VerackBody : Body
	{

		#region implemented abstract members of Body

		public override uint GetPayloadSize ()
		{
			return 0;
		}

		public override void Inflate (System.IO.Stream input)
		{
			// No body
		}

		public override byte[] Serialize ()
		{
			return new byte[0];
		}

		#endregion
	}
}

