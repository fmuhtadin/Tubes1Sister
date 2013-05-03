using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using GunbondLibrary;
using System.Windows.Forms;
using System.Threading;

namespace PeerModule
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

    public class PeerOptions
    {
        public int MAX_PEER;

        public PeerOptions()
        {
            this.MAX_PEER = 8;
        }
    }

    class GunbondPeer : IPeer
    {
        public Socket clientSocket;
        public Socket trackerSocket;
        public Socket sListener;
        public bool isConnectedToTracker;
        public IPAddress ipTracker;
        public IPAddress localIP;
        public List<IPAddress> listPendingRequest;
        public List<Room> listRoom;
        public int team;
        public List<IPAddress> listRoomPeers;
        public List<IPAddress> listTeam1Peers;
        public List<IPAddress> listTeam2Peers;

        private PeerForm peerForm;
        private byte[] buffer = new byte[1024];
        private String roomIdHeld;
        private List<ConnectionState> listPeerConnected;
        private ConnectionState connectedCreatorPeer;
        PeerOptions options;
        private int lastCommandCode;
        private int availablePort;

        public GunbondPeer(PeerForm newPeerForm)
        {
            peerForm = newPeerForm;
            listPeerConnected = new List<ConnectionState>();
            isConnectedToTracker = false;
            options = new PeerOptions();
            ipTracker = null;
            roomIdHeld = null;
            listPendingRequest = new List<IPAddress>();
            listRoom = new List<Room>();
            team = 0;
            listRoomPeers = new List<IPAddress>();
            listTeam1Peers = new List<IPAddress>();
            listTeam2Peers = new List<IPAddress>();
            availablePort = 3055;
        }

        public void InitSocket(String serverIPtext)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipLocalEndPoint = new IPEndPoint(IPAddress.Parse(peerForm.textPeerId.Text), availablePort);
            //sListener.Bind(ipLocalEndPoint);
            //clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            clientSocket.Bind(ipLocalEndPoint);
            availablePort++;

            IPAddress ipAddress = IPAddress.Parse(serverIPtext);
            //Server is listening on port 1000
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 3056);
            
            //Connect to the server
            clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);
        }

        public void InitListeningSocket()
        {
            isConnectedToTracker = true;
            //We are using TCP sockets
            sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Assign the any IP of the machine and listen on port number 1000
            //IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 3056);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(peerForm.textPeerId.Text), availablePort);
            availablePort++;
            //sListener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            //Bind and listen on the given address
            sListener.Bind(ipEndPoint);
            sListener.Listen(options.MAX_PEER);

            //Accept the incoming clients
            sListener.BeginAccept(new AsyncCallback(OnAccept), null);
        }

        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = sListener.EndAccept(ar);

                //Start listening for more clients
                sListener.BeginAccept(new AsyncCallback(OnAccept), null);

                //Once the client connects then start receiving the commands from her
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None,
                    new AsyncCallback(OnReceive), clientSocket);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GUNBOND ERROR 2",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                if (!isConnectedToTracker)
                {
                    clientSocket.EndConnect(ar);
                    ipTracker = (clientSocket.RemoteEndPoint as IPEndPoint).Address;
                    trackerSocket = clientSocket;

                    //We are connected so we login into the server
                    MessageData msgToSend = new MessageData();
                    msgToSend.pstr = "GunbondGame";
                    byte[] reserved = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    msgToSend.reservedBytes = reserved;
                    msgToSend.code = 135;
                    lastCommandCode = 135;

                    byte[] message = msgToSend.ToByte();

                    //Send the message to the server
                    clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), clientSocket);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Client Handshake", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                Socket currSocket = (Socket)ar.AsyncState;
                currSocket.EndSend(ar);
                currSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), currSocket);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SGSclient", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                Socket currSocket = (Socket)ar.AsyncState;
                currSocket.EndReceive(ar);

                MessageData msgReceived = new MessageData(buffer);

                MessageData msgToSend = new MessageData();
                byte[] message;
                msgToSend.pstr = msgReceived.pstr;
                msgToSend.reservedBytes = msgReceived.reservedBytes;

                //Accordingly process the message received
                switch (msgReceived.code)
                {
                    case 135:
                        lastCommandCode = 135;
                        if (!isConnectedToTracker)
                        {
                            peerForm.setMessagesText(msgReceived.pstr + Encoding.UTF8.GetString(new byte[] { msgReceived.code }));
                            peerForm.setPeerIdText(msgReceived.peerId.ToString());
                            localIP = msgReceived.peerId;
                        }
                        else
                        {
                            listPeerConnected.Add(new ConnectionState(currSocket));
                            msgToSend.code = 127;
                            message = msgToSend.ToByte();
                            currSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), currSocket);
                        }
                        break;

                    case 182:
                        //Keep alive
                        break;

                    case 255:
                        //create
                        break;

                    case 254:
                        //list
                        break;

                    case 200:
                        //room
                        List<String> listroomreceived = new List<string>();
                        listRoom.Clear();
                        for (int i = 0; i < msgReceived.listRoom.Count; i++)
                        {
                            listRoom.Add(msgReceived.listRoom[i]);
                            listroomreceived.Add(msgReceived.listRoom[i].roomId);
                        }
                        peerForm.setRoomListBox(listroomreceived);
                        break;

                    case 127:
                        //success
                        switch (lastCommandCode)
                        {
                            case 255:
                                //Success Create Room
                                listRoomPeers.Add(localIP);
                                peerForm.DisableRoomButton();
                                UpdateForm();
                                peerForm.setMessagesText("Create Room Successful");
                                peerForm.EnableStartGame();
                                break;
                            case 253:
                                //Success Join Room
                                if ((currSocket.RemoteEndPoint as IPEndPoint).Equals(trackerSocket.RemoteEndPoint as IPEndPoint))
                                {
                                    //from tracker
                                    lastCommandCode = 253;
                                    msgToSend.pstr = "GunbondGame";
                                    byte[] reserved = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                                    msgToSend.reservedBytes = reserved;
                                    msgToSend.code = 253;
                                    msgToSend.peerId = localIP;
                                    msgToSend.roomId = peerForm.GetSelectedRoomId();
                                    message = msgToSend.ToByte();

                                    IPAddress destCreatorPeer = null;
                                    foreach (Room room in listRoom)
                                    {
                                        if (room.roomId.Equals(peerForm.GetSelectedRoomId())) 
                                        {
                                            destCreatorPeer = room.creatorId;
                                        }
                                    }

                                    Socket clSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                                    //Server is listening on port 1000
                                    IPEndPoint ipEndPoint = new IPEndPoint(destCreatorPeer, 3056);
                                    IPEndPoint ipLocalEndPoint = new IPEndPoint(localIP, availablePort);
                                    availablePort++;
                                    //clSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                    clSock.Bind(ipLocalEndPoint);

                                    //Connect to the server
                                    clSock.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);

                                    //Send the message to the server
                                    clSock.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), clSock);
                                }
                                else
                                {
                                    //from superpeer
                                    ConnectionState peerConn = new ConnectionState(currSocket);
                                    listPeerConnected.Add(peerConn);
                                    connectedCreatorPeer = peerConn;
                                    UpdateForm();
                                    peerForm.setMessagesText("Join Room Successful");
                                    peerForm.DisableRoomButton();
                                    msgToSend.code = 103;
                                    message = msgToSend.ToByte();
                                    currSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), currSocket);
                                }
                                break;
                            case 135:
                                msgToSend.code = 115;
                                message = msgToSend.ToByte();
                                connectedCreatorPeer.socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), connectedCreatorPeer.socket);
                                break;
                        }
                        break;

                    case 128:
                        //failed
                        switch (lastCommandCode)
                        {
                            case 255:
                                peerForm.setMessagesText("Create Room UNsuccessful");
                                break;
                            case 253:
                                peerForm.setMessagesText("Join Room UNsuccessful");
                                break;
                        }
                        break;

                    case 253:
                        //join
                        if (ipTracker.Equals((currSocket.RemoteEndPoint as IPEndPoint).Address))
                        {
                            //from tracker
                            if (listPeerConnected.Count < options.MAX_PEER)
                            {
                                bool isIPAlreadyConnected = false;
                                for (int i = 0; i < listPeerConnected.Count; i++)
                                {
                                    if ((listPeerConnected[i].socket.RemoteEndPoint as IPEndPoint).Equals((currSocket.RemoteEndPoint as IPEndPoint).Address)) 
                                    {
                                        msgToSend.code = 128;
                                        message = msgToSend.ToByte();
                                        currSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                                    new AsyncCallback(OnSend), currSocket);
                                        isIPAlreadyConnected = true;
                                        break;
                                    }
                                }
                                if (!isIPAlreadyConnected)
                                {
                                    listPendingRequest.Add(msgReceived.peerId);
                                    msgToSend.code = 127;
                                    message = msgToSend.ToByte();
                                    currSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                                new AsyncCallback(OnSend), currSocket);
                                }
                            }
                            else
                            {
                                msgToSend.code = 128;
                                message = msgToSend.ToByte();
                                currSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                            new AsyncCallback(OnSend), currSocket);
                            }
                        }
                        else
                        {
                            //from peer
                            int q = 0;
                            bool valid = false;
                            foreach (IPAddress ip in listPendingRequest)
                            {
                                if (ip.Equals((currSocket.RemoteEndPoint as IPEndPoint).Address))
                                {
                                    valid = true;
                                    listRoomPeers.Add(ip);
                                    listPeerConnected.Add(new ConnectionState(currSocket));
                                    msgToSend.code = 127;
                                    message = msgToSend.ToByte();
                                    currSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                                new AsyncCallback(OnSend), currSocket);
                                    listPendingRequest.RemoveAt(q);
                                    
                                    break;
                                }
                                q++;
                            }
                            if (!valid)
                            {
                                msgToSend.code = 128;
                                message = msgToSend.ToByte();
                                currSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                            new AsyncCallback(OnSend), currSocket);
                            }
                        }

                        break;

                    case 252:
                        //start
                        break;

                    case 235:
                        //quit
                        break;

                    case 100:
                        //send peer ip list
                        listRoomPeers = msgReceived.listIPAddress;
                        listTeam1Peers = msgReceived.listIPTeam1;
                        listTeam2Peers = msgReceived.listIPTeam2;
                        UpdateForm();
                        if (lastCommandCode == 101 || lastCommandCode == 102)
                        {
                            peerForm.DisableTeamButton();
                        }
                        break;

                    case 103:
                        //request peer ip list
                        SendPeersList();
                        UpdateForm();
                        break;

                    case 101:
                        //request join team 1
                        if (listTeam1Peers.Count < 4)
                        {
                            listTeam1Peers.Add((currSocket.RemoteEndPoint as IPEndPoint).Address);
                            SendPeersList();
                            UpdateForm();
                        }
                        else
                        {
                            msgToSend.code = 128;
                            message = msgToSend.ToByte();
                            currSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                        new AsyncCallback(OnSend), currSocket);
                        }
                        break;

                    case 102:
                        //request join team 2
                        if (listTeam2Peers.Count < 4)
                        {
                            listTeam2Peers.Add((currSocket.RemoteEndPoint as IPEndPoint).Address);
                            SendPeersList();
                            UpdateForm();
                        }
                        else
                        {
                            msgToSend.code = 128;
                            message = msgToSend.ToByte();
                            currSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                        new AsyncCallback(OnSend), currSocket);
                        }
                        break;
                    case 110:
                        int number = GetSelfPositionNumber();
                        if (number > 10 && number < 20)
                        {
                            switch (number % 10)
                            {
                                case 1:
                                    if (listTeam1Peers.Count>1)
                                        HandshakePeer(listTeam1Peers[1]);
                                    if (listTeam1Peers.Count > 2)
                                        HandshakePeer(listTeam1Peers[2]);
                                    if (listTeam2Peers.Count > 0)
                                        HandshakePeer(listTeam2Peers[0]);
                                    break;
                                case 2:
                                    if (listTeam1Peers.Count > 0)
                                        HandshakePeer(listTeam1Peers[0]);
                                    if (listTeam1Peers.Count > 3)
                                        HandshakePeer(listTeam1Peers[3]);
                                    if (listTeam2Peers.Count > 1)
                                        HandshakePeer(listTeam2Peers[1]);
                                    break;
                                case 3:
                                    if (listTeam1Peers.Count > 0)
                                        HandshakePeer(listTeam1Peers[0]);
                                    if (listTeam1Peers.Count > 3)
                                        HandshakePeer(listTeam1Peers[3]);
                                    if (listTeam2Peers.Count > 2)
                                        HandshakePeer(listTeam2Peers[2]);
                                    break;
                                case 4:
                                    if (listTeam1Peers.Count > 1)
                                        HandshakePeer(listTeam1Peers[1]);
                                    if (listTeam1Peers.Count > 2)
                                        HandshakePeer(listTeam1Peers[2]);
                                    if (listTeam2Peers.Count > 3)
                                        HandshakePeer(listTeam2Peers[3]);
                                    break;
                            }
                        }
                        else if (number > 20)
                        {
                            switch (number % 10)
                            {
                                case 1:
                                    if (listTeam2Peers.Count > 1)
                                        HandshakePeer(listTeam2Peers[1]);
                                    if (listTeam2Peers.Count > 2)
                                        HandshakePeer(listTeam2Peers[2]);
                                    if (listTeam1Peers.Count > 0)
                                        HandshakePeer(listTeam1Peers[0]);
                                    break;
                                case 2:
                                    if (listTeam2Peers.Count > 0)
                                        HandshakePeer(listTeam2Peers[0]);
                                    if (listTeam2Peers.Count > 3)
                                        HandshakePeer(listTeam2Peers[3]);
                                    if (listTeam1Peers.Count > 1)
                                        HandshakePeer(listTeam1Peers[1]);
                                    break;
                                case 3:
                                    if (listTeam2Peers.Count > 0)
                                        HandshakePeer(listTeam2Peers[0]);
                                    if (listTeam2Peers.Count > 3)
                                        HandshakePeer(listTeam2Peers[3]);
                                    if (listTeam1Peers.Count > 2)
                                        HandshakePeer(listTeam1Peers[2]);
                                    break;
                                case 4:
                                    if (listTeam2Peers.Count > 1)
                                        HandshakePeer(listTeam2Peers[1]);
                                    if (listTeam2Peers.Count > 2)
                                        HandshakePeer(listTeam2Peers[2]);
                                    if (listTeam1Peers.Count > 3)
                                        HandshakePeer(listTeam1Peers[3]);
                                    break;
                            }
                        }
                        msgToSend.code = 115;
                        message = msgToSend.ToByte();
                        connectedCreatorPeer.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                        new AsyncCallback(OnSend), connectedCreatorPeer.socket);
                        break;
                    case 115:
                        for (int i = 0; i < listPeerConnected.Count; i++)
                        {
                            if ((currSocket.RemoteEndPoint as IPEndPoint).Address.Equals((listPeerConnected[i].socket.RemoteEndPoint as IPEndPoint).Address))
                            {
                                if (i == listPeerConnected.Count - 1)
                                {
                                    InitSuperPeerHyperCube();
                                }
                                else
                                {
                                    SendInitHyperCubeCommand(i+1);
                                }
                            }
                        }
                        break;
                    case 117:
                        for (int i = 0; i < listPeerConnected.Count; i++)
                        {
                            if ((connectedCreatorPeer.socket.RemoteEndPoint as IPEndPoint).Address.Equals((listPeerConnected[i].socket.RemoteEndPoint as IPEndPoint).Address))
                            {
                                listPeerConnected.RemoveAt(i);
                                connectedCreatorPeer.socket.Shutdown(SocketShutdown.Both);
                                connectedCreatorPeer.socket.Close();
                                connectedCreatorPeer = null;
                            }
                        }
                        break;
                    case 190:
                        msgToSend.code = 190;
                        message = msgToSend.ToByte();
                        SendCube(message, (currSocket.RemoteEndPoint as IPEndPoint).Address);
                        RunGame();
                        break;
                    case 70:
                        msgToSend.code = 70;
                        msgToSend.peerId = msgReceived.peerId;
                        msgToSend.state = msgReceived.state;
                        message = msgToSend.ToByte();
                        SendCube(message, (currSocket.RemoteEndPoint as IPEndPoint).Address);
                        UpdateOtherPlayer(msgReceived.peerId,msgReceived.state);
                        break;
                }

                //buffer = new byte[1024];

                if (!isConnectedToTracker)
                {
                    InitListeningSocket();
                }
                else
                {
                    currSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), currSocket);
                }
            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GUNBOND ERROR: ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CreateRoom(String idroom, int maxplayer) {
            lastCommandCode = 255;
            MessageData msgToSend = new MessageData();
            msgToSend.pstr = "GunbondGame";
            byte[] reserved = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            msgToSend.reservedBytes = reserved;
            msgToSend.code = 255;
            msgToSend.peerId = localIP;
            msgToSend.maxPlayerNum = maxplayer;
            msgToSend.roomId = idroom;
            roomIdHeld = idroom;

            byte[] message = msgToSend.ToByte();

            //Send the message to the server
            trackerSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), trackerSocket);
        }

        public void ListRoom()
        {
            lastCommandCode = 254;
            MessageData msgToSend = new MessageData();
            msgToSend.pstr = "GunbondGame";
            byte[] reserved = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            msgToSend.reservedBytes = reserved;
            msgToSend.code = 254;
            msgToSend.peerId = localIP;

            byte[] message = msgToSend.ToByte();

            //Send the message to the server
            trackerSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), trackerSocket);
        }

        public void ClosePeer()
        {
            sListener.Shutdown(SocketShutdown.Both);
            sListener.Close();
        }

        public void Join()
        {
            //MessageBox.Show(peerForm.lbRoom.Items[peerForm.lbRoom.SelectedIndex].ToString());
            lastCommandCode = 253;
            MessageData msgToSend = new MessageData();
            msgToSend.pstr = "GunbondGame";
            byte[] reserved = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            msgToSend.reservedBytes = reserved;
            msgToSend.code = 253;
            msgToSend.peerId = localIP;
            msgToSend.roomId = peerForm.GetSelectedRoomId();
            byte[] message = msgToSend.ToByte();

            //Send the message to the server
            trackerSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), trackerSocket);
        }

        public void JoinTeam1()
        {
            if (roomIdHeld == null)
            {
                //self is not creator peer
                lastCommandCode = 101;
                MessageData msgToSend = new MessageData();
                msgToSend.pstr = "GunbondGame";
                byte[] reserved = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                msgToSend.reservedBytes = reserved;
                msgToSend.code = 101;
                byte[] message = msgToSend.ToByte();

                //Send the message to the superpeer
                connectedCreatorPeer.socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), connectedCreatorPeer.socket);
            }
            else
            {
                //self is creator peer
                if (listTeam1Peers.Count < 4)
                {
                    listTeam1Peers.Add(localIP);
                    SendPeersList();
                    UpdateForm();
                    peerForm.DisableTeamButton();
                }
            }
        }

        public void JoinTeam2()
        {
            if (roomIdHeld == null)
            {
                lastCommandCode = 102;
                MessageData msgToSend = new MessageData();
                msgToSend.pstr = "GunbondGame";
                byte[] reserved = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                msgToSend.reservedBytes = reserved;
                msgToSend.code = 102;
                byte[] message = msgToSend.ToByte();

                //Send the message to the superpeer
                connectedCreatorPeer.socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), connectedCreatorPeer.socket);
            }
            else
            {
                if (listTeam2Peers.Count < 4)
                {
                    listTeam2Peers.Add(localIP);
                    SendPeersList();
                    UpdateForm();
                    peerForm.DisableTeamButton();
                }
            }
        }

        public void SendPeersList() 
        {
            MessageData msgToSend = new MessageData();
            msgToSend.code = 100;
            msgToSend.ipCount = listRoomPeers.Count;
            msgToSend.team1Count = listTeam1Peers.Count;
            msgToSend.team2Count = listTeam2Peers.Count;
            msgToSend.listIPAddress = listRoomPeers;
            msgToSend.listIPTeam1 = listTeam1Peers;
            msgToSend.listIPTeam2 = listTeam2Peers;
            byte[] message = msgToSend.ToByte();
            foreach (ConnectionState cs in listPeerConnected)
            {
                cs.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                        new AsyncCallback(OnSend), cs.socket);
            }
        }

        public void StartGame()
        {
            SendInitHyperCubeCommand(0);
        }

        public void SendInitHyperCubeCommand(int peerIndex)
        {
            MessageData msgToSend = new MessageData();
            msgToSend.code = 110;
            byte[] message = msgToSend.ToByte();
            ConnectionState cs = listPeerConnected[peerIndex];
            cs.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                        new AsyncCallback(OnSend), cs.socket);
        }

        public void InitSuperPeerHyperCube()
        {
            int number = GetSelfPositionNumber();
            if (number > 10 && number < 20)
            {
                switch (number % 10)
                {
                    case 1:
                        //HandshakePeer(listTeam1Peers[1]);
                        //HandshakePeer(listTeam1Peers[2]);
                        //HandshakePeer(listTeam2Peers[0]);
                        if (listTeam1Peers.Count > 3)
                            CloseConnection(listTeam1Peers[3]);
                        if (listTeam2Peers.Count > 1)
                            CloseConnection(listTeam2Peers[1]);
                        if (listTeam2Peers.Count > 2)
                            CloseConnection(listTeam2Peers[2]);
                        if (listTeam2Peers.Count > 3)
                            CloseConnection(listTeam2Peers[3]);
                        break;
                    case 2:
                        //HandshakePeer(listTeam1Peers[0]);
                        //HandshakePeer(listTeam1Peers[3]);
                        //HandshakePeer(listTeam2Peers[1]);
                        if (listTeam1Peers.Count > 2)
                            CloseConnection(listTeam1Peers[2]);
                        if (listTeam2Peers.Count > 0)
                            CloseConnection(listTeam2Peers[0]);
                        if (listTeam2Peers.Count > 2)
                            CloseConnection(listTeam2Peers[2]);
                        if (listTeam2Peers.Count > 3)
                            CloseConnection(listTeam2Peers[3]);
                        break;
                    case 3:
                        //HandshakePeer(listTeam1Peers[0]);
                        //HandshakePeer(listTeam1Peers[3]);
                        //HandshakePeer(listTeam2Peers[2]);
                        if (listTeam1Peers.Count > 1)
                            CloseConnection(listTeam1Peers[1]);
                        if (listTeam2Peers.Count > 0)
                            CloseConnection(listTeam2Peers[0]);
                        if (listTeam2Peers.Count > 1)
                            CloseConnection(listTeam2Peers[1]);
                        if (listTeam2Peers.Count > 3)
                        CloseConnection(listTeam2Peers[3]);
                        break;
                    case 4:
                        //HandshakePeer(listTeam1Peers[1]);
                        //HandshakePeer(listTeam1Peers[2]);
                        //HandshakePeer(listTeam2Peers[3]);
                        if (listTeam1Peers.Count > 0)
                            CloseConnection(listTeam1Peers[0]);
                        if (listTeam2Peers.Count > 0)
                            CloseConnection(listTeam2Peers[0]);
                        if (listTeam2Peers.Count > 1)
                            CloseConnection(listTeam2Peers[1]);
                        if (listTeam2Peers.Count > 2)
                            CloseConnection(listTeam2Peers[2]);
                        break;
                }
            }
            else if (number > 20)
            {
                switch (number % 10)
                {
                    case 1:
                        if (listTeam2Peers.Count > 3)
                            CloseConnection(listTeam2Peers[3]);
                        if (listTeam1Peers.Count > 1)
                            CloseConnection(listTeam1Peers[1]);
                        if (listTeam1Peers.Count > 2)
                            CloseConnection(listTeam1Peers[2]);
                        if (listTeam1Peers.Count > 3)
                            CloseConnection(listTeam1Peers[3]);
                        break;
                    case 2:
                        if (listTeam2Peers.Count > 2)
                            CloseConnection(listTeam2Peers[2]);
                        if (listTeam1Peers.Count > 0)
                            CloseConnection(listTeam1Peers[0]);
                        if (listTeam1Peers.Count > 2)
                            CloseConnection(listTeam1Peers[2]);
                        if (listTeam1Peers.Count > 3)
                            CloseConnection(listTeam1Peers[3]);
                        break;
                    case 3:
                        if (listTeam2Peers.Count > 1)
                            CloseConnection(listTeam2Peers[1]);
                        if (listTeam1Peers.Count > 0)
                            CloseConnection(listTeam1Peers[0]);
                        if (listTeam1Peers.Count > 1)
                            CloseConnection(listTeam1Peers[1]);
                        if (listTeam1Peers.Count > 3)
                            CloseConnection(listTeam1Peers[3]);
                        break;
                    case 4:
                        if (listTeam2Peers.Count > 0)
                            CloseConnection(listTeam2Peers[0]);
                        if (listTeam1Peers.Count > 0)
                            CloseConnection(listTeam1Peers[0]);
                        if (listTeam1Peers.Count > 1)
                            CloseConnection(listTeam1Peers[1]);
                        if (listTeam1Peers.Count > 2)
                            CloseConnection(listTeam1Peers[2]);
                        break;
                }
            }
            InitRunGame();
        }

        public void CloseConnection(IPAddress ip)
        {
            for (int i = 0; i < listPeerConnected.Count; i++)
            {
                if (ip.Equals((listPeerConnected[i].socket.RemoteEndPoint as IPEndPoint).Address)) 
                {
                    MessageData msgToSend = new MessageData();
                    msgToSend.code = 117;
                    byte[] message = msgToSend.ToByte();
                    ConnectionState cs = listPeerConnected[i];
                    cs.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                new AsyncCallback(OnSend), cs.socket);
                    cs.socket.Shutdown(SocketShutdown.Both);
                    cs.socket.Close();
                    listPeerConnected.RemoveAt(i);
                }
            }
        }

        public void RunGame() 
        {
            peerForm.StartGame();
        }

        public void InitRunGame()
        {
            MessageData msgToSend = new MessageData();
            msgToSend.code = 190;
            byte[] message = msgToSend.ToByte();
            foreach (ConnectionState cs in listPeerConnected)
            {
                cs.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                new AsyncCallback(OnSend), cs.socket);
            }
            RunGame();
        }

        public void SendCube(byte[] message, IPAddress ipsender) 
        {
            foreach (ConnectionState cs in listPeerConnected)
            {
                if (!(cs.socket.RemoteEndPoint as IPEndPoint).Address.Equals(ipsender))
                {
                    cs.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                new AsyncCallback(OnSend), cs.socket);
                }
            }
        }

        public void HandshakePeer(IPAddress ip) 
        {
            lastCommandCode = 135;
            bool isAlreadyConnected = false;
            foreach (ConnectionState cs in listPeerConnected)
            {
                if (ip.Equals((cs.socket.RemoteEndPoint as IPEndPoint).Address))
                {
                    isAlreadyConnected = true;
                }
            }
            if (!isAlreadyConnected)
            {
                MessageData msgToSend = new MessageData();
                msgToSend.code = 135;
                byte[] message = msgToSend.ToByte();
                Socket clSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint ipEndPoint = new IPEndPoint(ip, 3056);
                IPEndPoint ipLocalEndPoint = new IPEndPoint(localIP, availablePort);
                availablePort++;
                clSock.Bind(ipLocalEndPoint);

                clSock.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);

                clSock.BeginSend(message, 0, message.Length, SocketFlags.None,
                            new AsyncCallback(OnSend), clSock);
            }
        }

        public int GetSelfPositionNumber()
        {
            for (int i = 0; i < listTeam1Peers.Count; i++)
            {
                if (listTeam1Peers[i].Equals(localIP))
                {
                    return (11 + i);
                }
            }
            for (int i = 0; i < listTeam2Peers.Count; i++)
            {
                if (listTeam2Peers[i].Equals(localIP))
                {
                    return (21 + i);
                }
            }
            return 0;
        }

        public void UpdateForm()
        {
            peerForm.setRoomPeersListBox(listRoomPeers);
            peerForm.setTeam1PeersListBox(listTeam1Peers);
            peerForm.setTeam2PeersListBox(listTeam2Peers);
        }


        public void SendPosition(int state)
        {
            MessageData msgToSend = new MessageData();
            msgToSend.code = 70;
            msgToSend.peerId = localIP;
            msgToSend.state = state;
            byte[] message = msgToSend.ToByte();
            foreach (ConnectionState cs in listPeerConnected)
            {
                cs.socket.BeginSend(message, 0, message.Length, SocketFlags.None,new AsyncCallback(OnSend), cs.socket);
            }
        }

        public void UpdateOtherPlayer(IPAddress ip, int state)
        {
            peerForm.UpdateOtherPlayer(ip, state);
        }

        public List<IPAddress> GetListTeam1()
        {
            return listTeam1Peers;
        }

        public List<IPAddress> GetListTeam2()
        {
            return listTeam2Peers;
        }

        public IPAddress GetSelfIP()
        {
            return localIP;
        }

    }
}
