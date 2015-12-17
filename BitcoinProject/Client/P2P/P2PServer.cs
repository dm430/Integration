using System;
using System.Security.Policy;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Models;
using System.Threading;
using System.Linq;

namespace P2P
{
	public class P2PServer
	{
		private MessageHandler messageHandler { get; set; }

		public P2PServer(MessageHandler handler)
        {
			messageHandler = handler;
        }

		public void Go(){
			Thread t = new Thread (Start);
			t.Start ();
		}

		void Start(){
			// Establish the local endpoint for the socket.
			IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
			IPAddress ipAddress = ipHostEntry.AddressList.First( x => x.AddressFamily == AddressFamily.InterNetwork && x.GetAddressBytes ()[2] == 3);
			var ipEndPoint = new IPEndPoint(ipAddress, Constants.DEFAULT_NETWORK_PORT);

			// Create a TCP/IP socket.
			var listener = new TcpListener(ipEndPoint);

			// Bind the socket to the local endpoint and 
			// listen for incoming connections.
			try
			{
				// Start listening for connections.
				while (true)
				{
                    listener.Start();
					//Console.WriteLine("Listening for peer messages...");

					try{
						// Program is suspended while waiting for an incoming connection.
						using(var stream = listener.AcceptTcpClient ()){
							Message message = new Message(NetworkMode.MAIN, stream.GetStream ());
							//Console.WriteLine("Recieved message with command " + message.CommandName);
							var methods = typeof(MessageHandler).GetMethods();
							var method = methods
								.Where(x => x.GetCustomAttributes(false) != null &&
									x.GetCustomAttributes(false).OfType<HandlesMessage>() != null &&
									x.GetCustomAttributes(false).OfType<HandlesMessage>().ToArray().Count() > 0 &&
									x.GetCustomAttributes(false).OfType<HandlesMessage>().First().Value == message.CommandName).First();
							method.Invoke(messageHandler, new object[]{ stream.Client, message, stream.GetStream () });
						}
					} catch (Exception e){
						Console.Error.WriteLine (e.ToString ());
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}

