using System;

namespace P2P
{
	/**
	 * Used to mark a method in the
	 * MessageHandler class that handles
	 * a specific type of messsage.
	 **/
	[AttributeUsage(AttributeTargets.Method)]
	public class HandlesMessage : Attribute
	{
		public string Value { get; set; }

		public HandlesMessage(string value){
			Value = value;
		}
	}
}

