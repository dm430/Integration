using Models.Body;
using System;
using System.Net;
using System.IO;
using System.Security.Principal;

namespace Models
{
	/**
	 * Currently only for holding IP Address
	 * but possibility for later expansion
	 **/
	public class Peer
	{

		public IPEndPoint Ip { get; set; }

        public VersionBody Version { get; set; }

		public override bool Equals (object obj)
		{
			if(!(obj is Peer)){
				return false;
			}

			Peer peer = (Peer)obj;
			return Ip == peer.Ip || Ip.Equals (peer.Ip);
		}
	}
}

