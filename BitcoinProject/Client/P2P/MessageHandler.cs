using Models;
using System;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;
using Models.Body;
using System.Net;
using System.Linq;
using UserModels;
using System.Security.Policy;

namespace P2P
{
    /**
     * This class handles all incoming messages.
     * Only messages that are unsolicited or
     * requests should be implemented.
     * 
     * Possible messages:
     * txtmsg
     * messages
     * reg
     * regs
     * getreg
     * getmsg
     * cmprhash
     * 
     * Messages that will only be in response:
     * messages
     * regs
     * 
     **/
    public class MessageHandler
	{
        public static Random random = new Random();
        public P2PMessenger messenger { get; set; }
        public List<TextMessage> MessageHistory { get; set; }
		public List<RegistrationBody> Registrations { get; set; }
        public List<Peer> Peers { get; set; }
        public int Version { get; set; }
		private List<INetworkListener> listeners = new List<INetworkListener>();

		public MessageHandler(P2PMessenger messenger)
        {
			this.messenger = messenger;
            Peers = messenger.FetchPeers();
            MessageHistory = new List<TextMessage>();
			Registrations = new List<RegistrationBody> ();
            Version = Constants.DEFAULT_NETWORK_VERSION;
        }

		#region Listeners

		public void RegisterListener(INetworkListener listener){
			listeners.Add (listener);
		}

		#endregion

        #region Data Messages

        [HandlesMessage("txtmsg")]
		public void handleTextMessage(Socket socket, Message message, Stream stream)
        {

			stream.Close ();
			socket.Close ();

			if(MessageHistory.Contains (message.Body)) {
				return;
			}
				
			MessageHistory.Add ((TextMessage)message.Body);

			TextMessage text = (TextMessage)message.Body;

			InstantMessage msg = new InstantMessage () {
				Text = text.Text,
				UserName = text.UserHandle,
				Timestamp = text.Timestamp,
				Hash = text.Hash
			};

			foreach(INetworkListener l in listeners){
				l.OnMessageReceived (msg);
			}

			messenger.SendToAll (message, Peers);
        }

        [HandlesMessage("reg")]
		public void handleRegistration(Socket socket, Message message, Stream stream)
        {

			if(Registrations.Contains ((RegistrationBody)message.Body)){
				RegistrationBody reg = (RegistrationBody)message.Body;
				Registrations.Add (reg);

				UserRegistration registration = new UserRegistration () {
					UserName = 	reg.UserHandle,
					PublicKey = reg.PublicKey,
					Timestamp = reg.Timestamp
				};

				foreach(INetworkListener l in listeners){
					l.OnRegistrationReceived (registration);
				}
			}
        }

        [HandlesMessage("getreg")]
		public void handleGetRegistrationBody(Socket socket, Message message, Stream stream)
        {
			GetRegistrationBody body = (GetRegistrationBody)message.Body;

			RegistrationBody response = Registrations.First<RegistrationBody> (x => x.UserHandle.Equals (body.UserHandle));

			Message msg = new Message ();
			msg.Body = response;
			msg.CommandName = "reg";

			messenger.SendMessage (socket, msg);
        }

        [HandlesMessage("cmprhash")]
		public void handleCompareHashes(Socket socket, Message message, Stream stream)
        {
            // WARNING: this will only work if you have implemented hashes in your messages

			CompareHashesBody body = (CompareHashesBody)message.Body;

			if(!body.Hashes.Any ()){

				MessagesBody response = new MessagesBody();
				response.Messages = new List<TextMessage> (MessageHistory);

				Message msg = new Message ();
				msg.Body = response;
				msg.CommandName = "messages";

			} else {
				throw new NotImplementedException ();
			}
        }

        /**
        * Bytes	Name		Data Type			Summary
        * ----------------------------------------------------
        * 4 	version 		int32_t 	Highest protocol understoon by transmitting node
        * 
        * @see https://bitcoin.org/en/developer-reference#version
        * 
        * This used to be bitcoin specific, but now it's just for
        * connecting to other peers. It works, so feel free to use
        * it. Also, you could clean it up or use something else.
        * We give you free reign.
        **/
        [HandlesMessage("version")]
        public void handleVersion(Socket socket, Message message, Stream stream)
        {
            //Console.WriteLine("Handling version");
            //TODO check these before casting
            IPEndPoint end = (IPEndPoint)socket.RemoteEndPoint;
			Peer newPeer = new Peer() { Ip = new IPEndPoint(end.Address, Constants.DEFAULT_NETWORK_PORT), Version = (VersionBody)message.Body };

			if(!Peers.Contains (newPeer)){
				Peers.Add(newPeer);
		        //Console.WriteLine("Added peer, Sending response...");  
			}

			Message response = new Message() { CommandName = "version" };
			ulong nonce = (ulong)MessageHandler.random.Next();//TODO store this somewhere the server can see it.
			response.Body = new VersionBody() {
				Version = this.Version,
				UserAgent = "neumont",
				AddrRecvIPAddress = Constants.ipv4Toipv6(((IPEndPoint)socket.RemoteEndPoint).Address.ToString()),
				AddrTransIpAddress = Constants.ipv4Toipv6(((IPEndPoint)socket.LocalEndPoint).Address.ToString()),
				AddrTransPort = Convert.ToInt16(Constants.DEFAULT_NETWORK_PORT),
				Nonce = nonce,
				StartHeight = 12
			};
			socket.Send(response.Serialize());
			//Console.WriteLine("Sent response");

            //Console.WriteLine("Sending verack...");
            //TODO if the version is good... otherwise reject
            Message verak = new Message() { CommandName = "verack", Body = new VerackBody() };
            socket.Send(verak.Serialize());
            
            Message recievedVerak = new Message(NetworkMode.MAIN, stream);
            if (recievedVerak.CommandName != "verack")
            {
                //Console.WriteLine("Did not reciever verack message, removing peer");
                //TODO remove peer and reject
            }
            else
            {
                //Console.WriteLine("Recieved verack message, keeping peer");
            }
        }
        #endregion

		public List<Peer> GetPeers(){
			return Peers;
		}
    }
}