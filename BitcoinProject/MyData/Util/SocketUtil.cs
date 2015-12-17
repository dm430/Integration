using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Util
{
	public static class SocketUtil
	{
		public static Socket output(){
			// Create a TCP/IP socket.
			var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			return listener;
		}
	}
}

