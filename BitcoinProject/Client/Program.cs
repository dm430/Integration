using System;
using P2P;
using Models;
using Models.Body;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            //List of things to include
            //- Server Socket that monitors incoming connections
            //- Thread that manages blockchain

            //var server = new P2PServer();
            // server.Go();

			P2PMessenger messenger = new P2PMessenger ();
			MessageHandler handler = new MessageHandler (messenger);
			P2PServer client = new P2PServer(handler);

			Console.Write ("Enter username: ");
			string username = Console.ReadLine ();

            client.Go();

			RegistrationBody body = new RegistrationBody ();
			body.UserHandle = username;
			body.PublicKey = "notimplementedaaaaaaaaaaaaaaaaaa";

			Message msg = new Message ();
			msg.Body = body;
			msg.CommandName = "reg";
			messenger.SendToAll (msg, handler.GetPeers ());
            
			while(true){

				string input = Console.ReadLine ();

				TextMessage send = new TextMessage ();
				send.UserHandle = username;
				send.Text = input;
				send.Hash = "notimplementedaaaaaaaaaaaaaaaaaa";

				msg = new Message ();
				msg.Body = send;
				msg.CommandName = "txtmsg";

				messenger.SendToAll (msg, handler.GetPeers ());
			}
        }
    }
}
