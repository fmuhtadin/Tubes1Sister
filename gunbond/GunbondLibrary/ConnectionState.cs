using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace GunbondLibrary
{
    public class ConnectionState
    {
        public Socket socket;
        public int timeToLive;

        public ConnectionState(Socket clsock)
        {
            this.socket = clsock;
            timeToLive = 10;
        }
    }
}
