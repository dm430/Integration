using Models;
using P2P;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Nodes
{
    public class FullNode
    {
        //some collection of network connections. I think these will be other workers.
        //thread that does mining, So it can be interrupted
            
            /*
                The worker will control things from main.
                On run it will try to connect to the server and ask for network connections
                when given an address it will connect to them and ask for the block chian.
                when the block chain is recieved it will begin trying to mine the next block in the thread for it
                Then it will listen for requests from the network, and manage itself. This is
                    making sure it has enough connections
                    listening for block updates
            */
        #region LocalMethods

        public void RequestConnectionsFromNetwork()
        {
            //ask your connections for more connections. If they already have too many they will pass you on to their other connections.
        }

        public void RequestBlockChainFromNetwork()
        {
            //request an updated copy of the block chain from your connections. Maybe send your most recent one and they will tell you you are already up to date if you are.
        }

        public void SendBlockToNetWork()
        {
            //when a block is compelted by this worker, send it to all of its connections.
        }

        public void TryCreateBlock()
        {
            //MINE!
        }

        public void VerifyBlock()
        {
            //When you get a block from other worker, verify it.
        }

        public void VerifyBlockChain()
        {
            //Verify the block chain you have is correct.
        }

        public void MakeTransaction()
        {
            //I really don't know what making a transaction does..
            //afterwards it should add it to its block. And maybe send that information to its connections?
        }
        #endregion

        //These methods are ones that are called by other workers.
        #region Remotemethods
        public void RequestConnections()
        {
            //return some of your connections.
            //If your connections already have too many, they will return their own connections instead.
            //Need to make sure this doesn't loop.
        }

        public void RequestBlockChain()
        {
            //return your block chain.
        }
        
        public void PassBlock()
        {
            //the block given was completed by another worker, verify it and add to block chain if it is ok.
        }
        #endregion
    }
}
