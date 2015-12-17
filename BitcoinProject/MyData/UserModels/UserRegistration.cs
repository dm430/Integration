using System;

namespace UserModels
{
	public class UserRegistration
	{
		public string UserName { get; set; }
		public string PublicKey { get; set; }
		public long Timestamp { get; set; }
	}
}