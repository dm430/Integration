using System;
using Util;

namespace Util
{
	public class UnsupportedCommandException : Exception
	{
		public UnsupportedCommandException ()
			: base()
		{
			
		}

		public UnsupportedCommandException (string msg)
			: base(msg)
		{
			
		}
	}
}

