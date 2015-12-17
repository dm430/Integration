using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P2P;
using Client;
using UserModels;

namespace POC {
    class Program {
        private P2PNode node;

        static void Main(string[] args) {
            new Program();
        }

        public Program() {
            Console.WriteLine("Please enter a user name.");
            string userName = Console.ReadLine();

            node = new P2PNode(new byte[] { 192, 168, 3, 234 });
            node.RegisterListener(new ChatListener());

            UserRegistration registration = new UserRegistration() {
                UserName = userName,
                PublicKey = "not implemented.................", // must be 32 characters long
                Timestamp = DateTime.Now.ToBinary()
            };

            node.RegisterUser(registration);

            bool isChatting = true;
            while (isChatting) {
                string message = Console.ReadLine();

                if (message == "exit") {
                    isChatting = false;
                } else {
                    sendMessage(message);
                }
            }
        }

        private void sendMessage(string message) {
            InstantMessage msg = new InstantMessage() {
                Text = message,
                UserName = "bob",
                Timestamp = DateTime.Now.ToBinary(),
                Hash = "not implemented................." // must be 32 characters long
            };

            node.Send(msg);
            Console.WriteLine("You: " + message + "\n");
        }
    }
}
