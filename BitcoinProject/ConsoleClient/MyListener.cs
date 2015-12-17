using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModels;

namespace ConsoleClient
{
    public class MyListener : INetworkListener
    {
        public void OnMessageReceived(InstantMessage msg)
        {
            Console.WriteLine(msg.UserName + "(" + msg.Timestamp + "): " + msg.Text);
        }

        public void OnRegistrationReceived(UserRegistration registration)
        {
            Console.WriteLine(registration.UserName+" has connected!");
        }
    }
}