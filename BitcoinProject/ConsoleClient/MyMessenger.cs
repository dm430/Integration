using Client;
using Models;
using Models.Body;
using P2P;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserModels;
using System.Threading;

namespace ConsoleClient
{
    public class MyMessenger
    {
        internal void Start()
        {
            //setup
            byte[] serverAddress = ParseIpFromConsole();
            Console.WriteLine("Received: " + serverAddress[0] + "," + serverAddress[1] + "," + serverAddress[2] + "," + serverAddress[3] + ".");
            P2PNode node = new P2PNode(serverAddress);

            //create and register listener
            MyListener HeyListen = new MyListener();
            node.RegisterListener(HeyListen);

            //register username from console
            Console.Write("Enter username: ");
			string username = Console.ReadLine();
            

            //register username to network
            UserRegistration myRegistration = new UserRegistration() 
            { 
                UserName = username, 
                Timestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), 
                PublicKey = "notimplementedaaaaaaaaaaaaaaaaaa" 
            };
            node.RegisterUser(myRegistration);
            
            while (true)
            {
				Thread.Sleep (10000);
                //capture input
				string input = Console.ReadLine();

                //send input to network
                InstantMessage messageToSend = new InstantMessage() 
                { 
                    Text = input, 
                    UserName = username, 
                    Timestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond), 
                    Hash = "12345678901234567890123456789012" 
                };
                node.Send(messageToSend);
            }
        }

        private byte[] ParseIpFromConsole()
        {
            byte[] ip = new byte[4];

            Console.WriteLine("Enter the Server Ip (###.###.###.###):");
            while(true)
            {
                string input = Console.ReadLine(); //"192.168.3.234";

                Match match = Regex.Match(input, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                if (match.Success)
                {
                    string s = match.Value;
                    string[] parts = s.Split('.');
                    for (int i = 0; i < ip.Count(); i++)
                    {
                        ip[i] = ConvertStringToByte(parts[i]);
                    }
                    break;
                }
                else
                {

                    Console.WriteLine("Invalid Ip, try again.");
                }
            }
            return ip;
        }

        private byte ConvertStringToByte(string p)
        {
            int temp = int.Parse(p);
            byte returnable = (byte)temp;
            return returnable;
        }
    }
}