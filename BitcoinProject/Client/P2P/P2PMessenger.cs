using System;
using Models;
using System.Collections.Generic;
using System.Net.Sockets;
using Models.Body;
using System.Net;
using Util;

namespace P2P
{
    public class P2PMessenger
    {

        public P2PMessenger()
        {

        }

        /**
		 * Fetches messages and stores them in the Peer list
		 **/
        public List<Peer> FetchPeers()
        {

            Socket serverConnection = SocketUtil.output();
            Console.WriteLine("Trying to connect to seed server...");
			Console.WriteLine ("Trying " + Constants.ipEndPoint.Address + " at port " + Constants.ipEndPoint.Port);
            serverConnection.Connect(Constants.ipEndPoint);

            List<Peer> peers = new List<Peer>();
            // Create new message
            var msg = new Message();

            // Fill properties for Serializing
            msg.Mode = NetworkMode.MAIN;
            msg.Checksum = "\0\0\0\0";
            msg.CommandName = "seedreq";

            // Create body and store it in message
            var body = new SeedRequestBody() { IPCount = (uint)10 };
            msg.Body = body;

            // Send serialized message
            Console.WriteLine("Sending Peer Seed Request");
            serverConnection.Send(msg.Serialize());

            // Expect a SeedResponse
            Console.WriteLine("Expecting Peer Seed Response");
            Message response = null;
            using (var input = new NetworkStream(serverConnection))
            {
                response = new Message(NetworkMode.MAIN, input);
            }

            if (response.Body is SeedResponseBody)
            {
                var seedResponse = (SeedResponseBody)response.Body;
                for (int i = 0; i < seedResponse.IpAddresses.GetLength(0); i++)
                {
                    byte[] ip = new byte[4];
                    for (int j = 0; j < 4; j++)
                    {
                        ip[j] = seedResponse.IpAddresses[i, j];
                    }
                    Peer peer = new Peer() { Ip = new IPEndPoint(new IPAddress(ip), Constants.DEFAULT_NETWORK_PORT) };
                    peers.Add(peer);
                }
            } // Else except state? Nah, no need to worry about that.
            //Console.WriteLine("Connecting to peers recieved...");
            for (int i = 0; i < peers.Count; i++)
            {
                var peer = peers[i];
                //Console.WriteLine("Trying to connect to peer at address " + peer.Ip.Address);
                Socket peerSocket = SocketUtil.output();
                try
                {
                    peerSocket.Connect(peer.Ip);
                }
                catch (SocketException e)
                {
                    peers.Remove(peer);
                    Console.WriteLine(e.Message);
                    i--;
                    continue;
                }
                //Console.WriteLine("Connected to peer, sending version number");
                Message version = new Message() { CommandName = "version" };
                ulong nonce = (ulong)MessageHandler.random.Next();//TODO store this somewhere the server can see it.
                version.Body = new VersionBody()
                {
                    Version = Constants.DEFAULT_NETWORK_VERSION,
                    UserAgent = "neumont",
                    AddrRecvIPAddress = Constants.ipv4Toipv6(peer.Ip.Address.ToString()),
                    AddrTransIpAddress = Constants.ipv4Toipv6(((IPEndPoint)serverConnection.LocalEndPoint).Address.ToString()),
                    AddrTransPort = Convert.ToInt16(Constants.DEFAULT_NETWORK_PORT),
                    Nonce = nonce,
                    StartHeight = 12
                };
                peerSocket.Send(version.Serialize());
                //Console.WriteLine("Sent version number");
                //TODO some kind of timeout, and if the timeout happens remove this peer.
                using (var stream = new NetworkStream(peerSocket))
                {
                    //Console.WriteLine("Listening for version");
                    Message peerVersion = new Message(NetworkMode.MAIN, stream);
                    //TODO check for types before casting
                    peer.Version = (VersionBody)peerVersion.Body;
                    //TODO if the version is good... otherwise reject
                    //Console.WriteLine("Recieved version, sending verack");
                    Message verak = new Message() { CommandName = "verack", Body = new VerackBody() };
                    peerSocket.Send(verak.Serialize());
                    //Console.WriteLine("Verak sent, connected to peer");
                }
            }
            return peers;
        }

        public void SendMessage(Socket socket, Message msg)
        {
            socket.Send(msg.Serialize());
        }

		public void SendToAll(Message msg, List<Peer> peers)
        {
			List<Peer> toRemove = new List<Peer> ();
			foreach(Peer peer in peers){
				Socket s = new Socket (SocketType.Stream, ProtocolType.Tcp);
				s.ReceiveTimeout = 500;
				s.SendTimeout = 500;
                try
                {
                    s.Connect(peer.Ip);
                    s.Send(msg.Serialize());
                    s.Close();
                }
                catch (Exception) {
					toRemove.Add (peer);
				}
			}
			peers.RemoveAll (x => toRemove.Contains (x)); 
        }

    }
}

