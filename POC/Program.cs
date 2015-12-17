using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModels;

namespace POC {
    class Program {
        private P2PNode node;

        static void Main(string[] args) {
            new Program();
        }

        public Program() {
            Console.Title = "Chat";
            Console.WriteLine("Please enter a user name.");
            string userName = Console.ReadLine();
            Console.Title = "Chat: Signed in as " + userName;

            node = new P2PNode(new byte[] { 192, 168, 3, 234 });
            node.RegisterListener(new ChatListener());

            UserRegistration registration = new UserRegistration() {
                UserName = userName,
                PublicKey = "not implemented.................", // must be 32 characters long
                Timestamp = DateTime.Now.ToBinary()
            };
            
            node.RegisterUser(registration);

            StartChat();
        }

        private void StartChat() {
            Console.Clear();
            Console.WriteLine(".::Welcome to the chat::.");
            bool isChatting = true;

            while (isChatting) {
                string message = Console.ReadLine();

                if (message == "exit") {
                    isChatting = false;
                } else {
                    SendMessage(message);
                }
            }

            Console.WriteLine("Good bye!");
            System.Threading.Thread.Sleep(2000);
            System.Environment.Exit(1);
        }

        private void SendMessage(string message) {
            InstantMessage msg = new InstantMessage() {
                Text = message,
                UserName = "bob",
                Timestamp = DateTime.Now.ToBinary(),
                Hash = "not implemented................." // must be 32 characters long
            };

            node.Send(msg);
            EchoMessage(message);
        }

        private void EchoMessage(string message) {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine("You: " + message);
        }
    }
}
