using System;
using P2P;
using UserModels;
using Models.Body;
using Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Client
{
	public class P2PNode
	{
		P2PMessenger messenger;
		P2PServer server;
		MessageHandler handler;

		public P2PNode (byte[] ServerAddress)
		{
			Constants.SEED_SERVER_ADDRESS = ServerAddress;
			messenger = new P2PMessenger ();
			handler = new MessageHandler (messenger);
			server = new P2PServer (handler);

			server.Go ();
		}

		public void RegisterListener(INetworkListener listener){
			handler.RegisterListener (listener);
		}

		public void Send(InstantMessage msg){
			TextMessage txtmsg = new TextMessage () {
				Hash = msg.Hash,
				Text = msg.Text,
				Timestamp = msg.Timestamp,
				UserHandle = msg.UserName
			};

			Message message = new Message () {
				Body = txtmsg,
				CommandName = "txtmsg",
				Mode = NetworkMode.MAIN
			};

			handler.MessageHistory.Add (txtmsg);
			messenger.SendToAll (message, handler.Peers);
		}

		public void RegisterUser(UserRegistration registration){
			RegistrationBody r = new RegistrationBody () {
				PublicKey = registration.PublicKey,
				Timestamp = registration.Timestamp,
				UserHandle = registration.UserName
			};

			Message msg = new Message () {
				Body = r,
				Checksum = "not implementedaaaaaaaaaaaaaaaaa",
				CommandName = "reg",
				Mode = NetworkMode.MAIN
			};

			messenger.SendToAll (msg, handler.Peers);
		}

		public List<UserRegistration> Registrations {
			get {
				List<UserRegistration> output = new List<UserRegistration> ();
				foreach(RegistrationBody r in handler.Registrations){
					var reg = new UserRegistration () {
						PublicKey = r.PublicKey,
						Timestamp = r.Timestamp,
						UserName = r.UserHandle
					};
					output.Add (reg);
				}
				return output;
			}
		}

		public List<InstantMessage> Messages {
			get {
				List<InstantMessage> msgs = new List<InstantMessage> ();
				foreach(TextMessage msg in handler.MessageHistory){
					var message = new InstantMessage () {
						Hash = msg.Hash,
						Text = msg.Text,
						Timestamp = msg.Timestamp,
						UserName = msg.UserHandle
					};
					msgs.Add (message);
				}
				return msgs;
			}
		}
	}
}

