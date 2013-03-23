using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets; 

namespace PeerModule
{
    class GunbondPeer
    {
        SocketPermission permission;
        Socket socketListener;
        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint ipEndPoint;

        public GunbondPeer()
        {
            permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
            
            ipHost = Dns.GetHostEntry("");
            ipAddr = ipHost.AddressList[0];
            ipEndPoint = new IPEndPoint(ipAddr, 4510);

            socketListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void JoinRoom()
        {
        }

        public void InitiateHandshake()
        {
        }

        public void GetListRoom()
        {
        }

        public void StartGame()
        {
        }

        public void QuitRoom()
        {
        }

    }
}
