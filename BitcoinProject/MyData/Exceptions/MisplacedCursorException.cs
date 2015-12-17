using System;
using System.ComponentModel.Design;

namespace Util
{
	public class MisplacedCursorException : Exception
	{
		public MisplacedCursorException ()
			: base()
		{
		}

		public MisplacedCursorException (string message)
			:base(message)
		{
			
		}
	}
}

