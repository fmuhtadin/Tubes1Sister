using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using GunbondLibrary;
using System.Windows.Forms;

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

    class GunbondPeer
    {
        public Socket clientSocket;
        public Socket trackerSocket;
        public Socket sListener;
        public bool isConnectedToTracker;
        public IPAddress ipTracker;
        public IPAddress localIP;
        public List<IPAddress> listPendingRequest;

        private PeerForm peerForm;
        private byte[] buffer = new byte[1024];
        private String roomIdHeld;
        private List<ConnectionState> listPeerConnected;
        private ConnectionState connectedCreatorPeer;
        PeerOptions options;
        private int lastCommandCode;

        public GunbondPeer(PeerForm newPeerForm)
        {
            peerForm = newPeerForm;
            listPeerConnected = new List<ConnectionState>();
            isConnectedToTracker = false;
            options = new PeerOptions();
            ipTracker = null;
            listPendingRequest = new List<IPAddress>();
        }

        public void InitSocket(String serverIPtext)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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
            sListener = new Socket(AddressFamily.InterNetwork,
                                      SocketType.Stream,
                                      ProtocolType.Tcp);

            //Assign the any IP of the machine and listen on port number 1000
            //IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 3056);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(peerForm.textPeerId.Text), 3056);

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
                clientSocket.EndConnect(ar);
                ipTracker = (clientSocket.RemoteEndPoint as IPEndPoint).Address;
                trackerSocket = clientSocket;

                //We are connected so we login into the server
                MessageData msgToSend = new MessageData();
                msgToSend.pstr = "GunbondGame";
                byte[] reserved = new byte[] {0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
                msgToSend.reservedBytes = reserved;
                msgToSend.code = 135;

                byte[] message = msgToSend.ToByte();

                //Send the message to the server
                clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
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
                clientSocket.EndSend(ar);
                clientSocket.BeginReceive(buffer,
                                          0,
                                          buffer.Length,
                                          SocketFlags.None,
                                          new AsyncCallback(OnReceive),
                                          null);
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
                clientSocket.EndReceive(ar);

                MessageData msgReceived = new MessageData(buffer);

                MessageData msgToSend = new MessageData();
                byte[] message;
                msgToSend.pstr = msgReceived.pstr;
                msgToSend.reservedBytes = msgReceived.reservedBytes;

                //Accordingly process the message received
                switch (msgReceived.code)
                {
                    case 135:
                        peerForm.setMessagesText(msgReceived.pstr+Encoding.UTF8.GetString( new byte[] {msgReceived.code}));
                        peerForm.setPeerIdText(msgReceived.peerId.ToString());
                        localIP = msgReceived.peerId;
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
                        for (int i = 0; i < msgReceived.listRoom.Count; i++)
                        {
                            listroomreceived.Add(msgReceived.listRoom[i].roomId);
                        }
                        peerForm.setRoomListBox(listroomreceived);
                        break;

                    case 127:
                        //success
                        switch (lastCommandCode)
                        {
                            case 255:
                                peerForm.setMessagesText("Create Room Successful");
                                break;
                        }
                        break;

                    case 128:
                        //failed
                        switch (lastCommandCode)
                        {
                            case 255:
                                roomIdHeld = null;
                                peerForm.setMessagesText("Create Room UNsuccessful");
                                break;
                        }
                        break;

                    case 253:
                        //join
                        if (ipTracker.Equals((clientSocket.RemoteEndPoint as IPEndPoint).Address))
                        {
                            if (listPeerConnected.Count < options.MAX_PEER)
                            {
                                listPendingRequest.Add(msgReceived.peerId);
                                msgToSend.code = 127;
                                message = msgToSend.ToByte();
                                clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                            new AsyncCallback(OnSend), clientSocket);
                            }
                            else
                            {
                                msgToSend.code = 128;
                                message = msgToSend.ToByte();
                                clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                            new AsyncCallback(OnSend), clientSocket);
                            }
                        }
                        else
                        {
                            int q = 0;
                            bool valid = false;
                            foreach (IPAddress ip in listPendingRequest)
                            {
                                if (ip.Equals((clientSocket.RemoteEndPoint as IPEndPoint).Address))
                                {
                                    valid = true;
                                    listPendingRequest.RemoveAt(q);
                                    msgToSend.code = 127;
                                    message = msgToSend.ToByte();
                                    clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                                new AsyncCallback(OnSend), clientSocket);
                                }
                                q++;
                            }
                            if (!valid)
                            {
                                msgToSend.code = 128;
                                message = msgToSend.ToByte();
                                clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                            new AsyncCallback(OnSend), clientSocket);
                            }
                        }

                        break;

                    case 252:
                        //start
                        break;

                    case 235:
                        //quit
                        break;


                    //case Command.Logout:
                    //    lstChatters.Items.Remove(msgReceived.strName);
                    //    break;

                    //case Command.Message:
                    //    break;

                    //case Command.List:
                    //    lstChatters.Items.AddRange(msgReceived.strMessage.Split('*'));
                    //    lstChatters.Items.RemoveAt(lstChatters.Items.Count - 1);
                    //    txtChatBox.Text += "<<<" + strName + " has joined the room>>>\r\n";
                    //    break;
                }


                buffer = new byte[1024];

                if (!isConnectedToTracker)
                {
                    InitListeningSocket();
                }
                else
                {

                    clientSocket.BeginReceive(buffer,
                                              0,
                                              buffer.Length,
                                              SocketFlags.None,
                                              new AsyncCallback(OnReceive),
                                              null);
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
            trackerSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
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
            trackerSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
        }

        public void ClosePeer()
        {
            sListener.Close();
        }

    }
}
