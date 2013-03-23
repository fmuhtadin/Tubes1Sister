using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets; 

namespace TrackerModule
{
    class GunbondTracker
    {
        SocketPermission permission;
        Socket sListener;
        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint ipEndPoint;
        
        public GunbondTracker()
        {
            permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
            
            ipHost = Dns.GetHostEntry("");
            ipAddr = ipHost.AddressList[0];
            ipEndPoint = new IPEndPoint(ipAddr, 4510);

            sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void SetMaxPeer()
        {
        }

        public void SetMaxRoom()
        {
        }

        public void LogOn()
        {
        }

        public void LogOff()
        {
        }

        public void Shutdown()
        {
        }
    }
}
