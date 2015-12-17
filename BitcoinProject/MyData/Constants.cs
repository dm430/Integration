using System;
using System.Net;

public static class Constants
{
	public static byte[] SEED_SERVER_ADDRESS {
		get {
			return _SEED_SERVER_ADDRESS;
		}
		set {
			_SEED_SERVER_ADDRESS = value;
			ipAddress = new IPAddress (_SEED_SERVER_ADDRESS);
			ipEndPoint = new IPEndPoint (ipAddress, 11000);
		}
	}
	private static byte[] _SEED_SERVER_ADDRESS = {192, 168, 3, 234};
    public static readonly int SEED_SERVER_PORT = 11000;
    public static IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
    //use the seed server address work with the server. Use the ipHostEntry to work locallys
    public static IPAddress ipAddress = 
        new IPAddress(SEED_SERVER_ADDRESS);
    // ipHostEntry.AddressList[1];
    public static IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 11000);
    public static readonly int DEFAULT_NETWORK_PORT = 8333;
    public static readonly int DEFAULT_NETWORK_VERSION = 70002;

    // Time to check for alive connections (4 a.m every day)
    public static readonly int Hour = 4;
    public static readonly int Minute = 0;

    public static string ipv4Toipv6(string ipv4)
    {
        string[] ipSections = ipv4.Split('.');
        string ipv6 = Convert.ToString(Convert.ToInt16(ipSections[0]), 16) +
            Convert.ToString(Convert.ToInt16(ipSections[1]), 16) +
            Convert.ToString(Convert.ToInt16(ipSections[2]), 16) +
            Convert.ToString(Convert.ToInt16(ipSections[3]), 16);
        ipv6 = ipv6.ToUpper();
        ipv6 = ipv6.Substring(0, 4) + ":" + ipv6.Substring(4);
        return ipv6;
    }
}

