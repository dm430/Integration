using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModels;

namespace POC {
    public class ChatListener : INetworkListener {
        public void OnMessageReceived(InstantMessage msg) {
            string message = "(" + msg.Timestamp + ") " + msg.UserName + ": " + msg.Text + "\n";
            Console.WriteLine(message);
        }

        public void OnRegistrationReceived(UserRegistration registration) {
            string message = registration.UserName + " Joing the room...";
            Console.WriteLine(message);
        }
    }
}
