using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Models;
using Models.Body;
using System.Linq;

namespace Server
{
    public class ServerClass
    {
        public static List<byte[]> IpAddresses { get; set; }

        public static void StartServer()
        {
            // Establish the local endpoint for the socket.
            IPAddress ipAddress = Constants.ipAddress;
            
            // Create a TCP/IP socket.
            var listener = new TcpListener(ipAddress, 11000);
            listener.Start();

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try
            {
                //listener.Bind(ipEndPoint);
                //listener.Listen(10);

                // Start listening for connections.
                while (true)
                {
                    foreach (var item in IpAddresses)
                    {
                        foreach (byte b in item)
                        {
                            Console.Write(b);
                        }
                        Console.WriteLine();
                    }

                    Console.WriteLine("Waiting for a connection...");

                    // Program is suspended while waiting for an incoming connection.
                    Socket com = listener.AcceptSocket();

                    Console.WriteLine("Connection recieved.");

                    var stream = new NetworkStream(com);

                    var message = new Message(NetworkMode.MAIN, stream);
                    Console.WriteLine("Loaded message");

                    #region Get connected client's IP and store it

                    // Get Client IP
                    var ip = ((IPEndPoint) com.RemoteEndPoint).Address.GetAddressBytes();

                    // Add to IP Addresses list
                    if (!IpAddresses.Contains(ip))
                    {
                        IpAddresses.Add(ip);
                    }

                    #endregion

                    #region Create returned message

                    var response = new Message();
                    var responseBody = new SeedResponseBody();

                    var seedBody = message.Body as SeedRequestBody;
                    if (seedBody != null)
                    {
                        var connections = GetConnections(seedBody.IPCount);
                        foreach (var con in connections)
                        {
                            Console.Write(con);
                            Console.WriteLine();
                        }
                        responseBody.IpAddresses = connections;
                    }

                    response.Mode = NetworkMode.MAIN;
                    response.CommandName = "seedresp";
                    response.Checksum = "\0\0\0\0";
                    response.Body = responseBody;

                    com.Send(response.Serialize());
                    Console.WriteLine("Sent response");

                    #endregion

                    com.Shutdown(SocketShutdown.Both);
                    com.Close();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }

        private static byte[,] GetConnections(uint totalConnection)
        {
            byte[][] addresses = IpAddresses.OrderBy(x => Guid.NewGuid()).Take((int)totalConnection).ToArray();

            var output = new byte[Math.Min(totalConnection, addresses.Length), 4];
            for (int i = 0; i < output.Length / 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    output[i, j] = addresses[i][j];
                }
            }
            return output;
        }

        static void Main(string[] args)
        {
#if DEBUG
            args = new[] { "192.168.3.234" };
#endif
            if (args.Length != 1){
				throw new Exception ("You forgot to give me the IP address that I should bind on. It probably starts with 192.168.3");
			}
			string[] ipchunks = args [0].Split ('.');
			if(ipchunks.Length != 4){
				throw new Exception ("I expected an ip address as the first argument, but got something else.");
			}
			byte[] serverAddress = new byte[4];

			for(int i = 0; i < ipchunks.Length; i++){
				serverAddress [i] = Byte.Parse (ipchunks [i]);
			}

			Constants.SEED_SERVER_ADDRESS = serverAddress;

            IpAddresses = new List<byte[]>();

            // Set up scheduler
            BackgroundWorker.StartScheduler();
            BackgroundWorker.ScheduleChecker(IpAddresses);

            StartServer();

            // Shut down scheduler
            BackgroundWorker.ShutdownScheduler();
        }
    }
}
