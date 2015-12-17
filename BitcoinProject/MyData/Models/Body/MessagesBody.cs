using Models.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using Util;
using Models.Body;

namespace Models.Body
{
	/**
	 * Sent in response to a
	 * cmprhash message.
	 * 
	 * Contains either a range of new
	 * Messages, or all Messages that
	 * were not found in the cmprhash
	 * message
	 **/
	[BodyCommand("messages")]
	public class MessagesBody : Body
	{
        public IEnumerable<TextMessage> Messages { get; set; }
        #region implemented abstract members of Body

        public override uint GetPayloadSize ()
		{
            uint size = 4;
            foreach(TextMessage text in Messages)
            {
                size += text.GetPayloadSize();
            }
            return size;
		}

		public override void Inflate (System.IO.Stream input)
		{
            int count = BinaryUtil.IntFromStream(input, 4);
            List<TextMessage> messages = new List<TextMessage>();
            while(count > 0)
            {
                TextMessage message = new TextMessage();
                message.Inflate(input);
                messages.Add(message);
                count--;
            }
            Messages = messages;
		}

		public override byte[] Serialize ()
		{
            byte[] data = new byte[GetPayloadSize()];
            int index = 4;
            Buffer.BlockCopy(new[] { Messages.Count() }, 0, data, 0, 4);
            foreach(TextMessage text in Messages)
            {
                int size = (int)text.GetPayloadSize();
                Buffer.BlockCopy(text.Serialize(), 0, data, index, size);
                index += size;
            }
            return data;
		}

		#endregion
	}
}

