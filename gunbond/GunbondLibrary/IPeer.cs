using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace GunbondLibrary
{
    public interface IPeer
    {
        void SendPosition(int state);
        void SendTurnOrder(List<int> turn);
        List<IPAddress> GetListTeam1();
        List<IPAddress> GetListTeam2();
        IPAddress GetSelfIP();
    }
}
