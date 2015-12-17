using System;

namespace UserModels
{
	public class InstantMessage
	{
		public string Text { get; set; }
		public string UserName { get; set; }
		public long Timestamp { get; set; }
		public string Hash { get; set; }
	}
}

