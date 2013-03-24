using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using GunbondLibrary;
using System.Windows.Forms; 

namespace TrackerModule
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

    //Tracker Options
    public class TrackerOptions {
        public int MAX_PEER;
        public int MAX_ROOM;

        public TrackerOptions()
        {
            this.MAX_PEER = 20;
            this.MAX_ROOM = 10;
        }
    }

    public struct RequestJoinState
    {
        public IPAddress creatorIP;
        public IPAddress requesterIP;

        public RequestJoinState(IPAddress creator, IPAddress requester)
        {
            this.creatorIP = creator;
            this.requesterIP = requester;
        }
    }

    public class GunbondTracker
    {
        //constant
        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 3056;

        TrackerOptions trackeroptions = new TrackerOptions();

        //data
        List<Room> listRoom;
        List<ConnectionState> listPeerConnection;
        List<RequestJoinState> listJoinRequest;

        byte[] buffer = new byte[BUFFER_SIZE];

        Socket sListener;

        TrackerForm form;
        
        public GunbondTracker(TrackerForm form1)
        {
            form = form1;
            listRoom = new List<Room>();
            listPeerConnection = new List<ConnectionState>();
            listJoinRequest = new List<RequestJoinState>();
            InitSocket();
        }

        public void InitSocket()
        {
            //We are using TCP sockets
            sListener = new Socket(AddressFamily.InterNetwork,
                                      SocketType.Stream,
                                      ProtocolType.Tcp);

            //Assign the any IP of the machine and listen on port number 1000
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, PORT_NUMBER);

            //Bind and listen on the given address
            sListener.Bind(ipEndPoint);
            sListener.Listen(trackeroptions.MAX_PEER);

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
                MessageBox.Show(ex.Message, "GUNBOND ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState;
                clientSocket.EndReceive(ar);

                //Transform the array of bytes received from the user into an
                //intelligent form of object Data
                MessageData msgReceived = new MessageData(buffer);

                

                //We will send this object in response the users request
                MessageData msgToSend = new MessageData();
                form.SetTextMessagesReceived(msgReceived.pstr + Encoding.UTF8.GetString(new byte[] {msgReceived.code}));

                byte[] message;

                //If the message is to login, logout, or simple text message
                //then when send to others the type of the message remains the same
                msgToSend.pstr = msgReceived.pstr;
                msgToSend.reservedBytes = msgReceived.reservedBytes;

                switch (msgReceived.code)
                {
                    case 135:

                        //When a user logs in to the server then we add her to our
                        //list of clients
                        msgToSend.code = 135;
                        IPAddress clientIP = (clientSocket.RemoteEndPoint as IPEndPoint).Address;
                        ConnectionState clState = new ConnectionState(clientSocket);
                        listPeerConnection.Add(clState);
                        msgToSend.peerId = clientIP;
                        Console.Out.WriteLine("connected client IP: " + clientIP);

                        message = msgToSend.ToByte();
                        clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                        new AsyncCallback(OnSend), clientSocket);

                        break;

                    case 182:
                        //reset time to live
                        foreach (ConnectionState cs in listPeerConnection)
                        {
                            if (cs.socket.Equals(clientSocket))
                            {
                                cs.timeToLive = 10;
                            }
                        }

                        //send back keep alive
                        msgToSend.code = 182;
                        msgToSend.peerId = (clientSocket.RemoteEndPoint as IPEndPoint).Address;

                        message = msgToSend.ToByte();
                        clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                        new AsyncCallback(OnSend), clientSocket);

                        break;

                    case 255:
                        //Create new room
                        if (listRoom.Count < trackeroptions.MAX_ROOM)
                        {
                            Room newRoom = new Room(msgReceived.roomId, (clientSocket.RemoteEndPoint as IPEndPoint).Address);
                            listRoom.Add(newRoom);

                            //send message success
                            msgToSend.code = 127;
                            message = msgToSend.ToByte();
                            clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                        new AsyncCallback(OnSend), clientSocket);
                        }
                        else
                        {
                            //send message failed
                            msgToSend.code = 128;
                            message = msgToSend.ToByte();
                            clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                        new AsyncCallback(OnSend), clientSocket);
                        }
                        
                        break;

                    case 254:
                        //Build the list message
                        msgToSend.code = 200;
                        msgToSend.roomCount = listRoom.Count;
                        List<Room> retList = new List<Room>();
                        foreach (Room room in listRoom)
                        {
                            retList.Add(room);
                        }
                        msgToSend.listRoom = retList;

                        message = msgToSend.ToByte();
                        clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                        new AsyncCallback(OnSend), clientSocket);

                        break;

                    case 200:
                        //Do nothing

                        break;

                    case 127:
                        //check pending join request
                        IPAddress requester = null;
                        int i = 0;
                        foreach (RequestJoinState rs in listJoinRequest) {
                            if (rs.creatorIP.Equals((clientSocket.RemoteEndPoint as IPEndPoint).Address)) {
                                requester = rs.requesterIP;
                                listJoinRequest.RemoveAt(i);
                                break;
                            }
                            i++;
                        }

                        //Send the message to the corresponding peer
                        if (requester != null)
                        {
                            msgToSend.code = 127;
                            message = msgToSend.ToByte();
                            foreach (ConnectionState cs in listPeerConnection)
                            {
                                if (requester.Equals((cs.socket.RemoteEndPoint as IPEndPoint).Address))
                                {
                                    cs.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                            new AsyncCallback(OnSend), clientSocket);
                                }
                            }
                        }

                        break;

                    case 128:
                        //check pending join request
                        IPAddress requester1 = null;
                        int j = 0;
                        foreach (RequestJoinState rs in listJoinRequest)
                        {
                            if (rs.creatorIP.Equals((clientSocket.RemoteEndPoint as IPEndPoint).Address))
                            {
                                requester1 = rs.requesterIP;
                                listJoinRequest.RemoveAt(j);
                                break;
                            }
                            j++;
                        }

                        //Send the message to the corresponding peer
                        if (requester1 != null)
                        {
                            msgToSend.code = 128;
                            message = msgToSend.ToByte();
                            foreach (ConnectionState cs in listPeerConnection)
                            {
                                if (requester1.Equals((cs.socket.RemoteEndPoint as IPEndPoint).Address))
                                {
                                    cs.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                            new AsyncCallback(OnSend), clientSocket);
                                }
                            }
                        }

                        break;

                    case 253:
                        //Send request to corresponding creator
                        msgToSend.code = 253;
                        msgToSend.peerId = msgReceived.peerId;
                        msgToSend.roomId = msgReceived.roomId;
                        message = msgToSend.ToByte();
                        foreach (ConnectionState cs in listPeerConnection)
                        {
                            foreach (Room room in listRoom)
                            {
                                if (room.creatorId.Equals((cs.socket.RemoteEndPoint as IPEndPoint).Address))
                                {
                                    //Add pending request
                                    RequestJoinState rjs = new RequestJoinState(room.creatorId,(clientSocket.RemoteEndPoint as IPEndPoint).Address);
                                    listJoinRequest.Add(rjs);

                                    cs.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                        new AsyncCallback(OnSend), clientSocket);
                                }
                            }
                        }

                        
                       


                        break;

                    case 252:

                        break;

                    case 235:

                        //When a user wants to log out of the server then we search for her 
                        //in the list of clients and close the corresponding connection

                        int nIndex = 0;
                        foreach (ConnectionState clientConn in listPeerConnection)
                        {
                            if (clientConn.socket.Equals(clientSocket))
                            {
                                listPeerConnection.RemoveAt(nIndex);
                                break;
                            }
                            ++nIndex;
                        }

                        msgToSend.code = 235;
                        msgToSend.peerId = (clientSocket.RemoteEndPoint as IPEndPoint).Address;
                        message = msgToSend.ToByte();
                        foreach (Room room in listRoom)
                        {
                            foreach (ConnectionState clConn in listPeerConnection)
                            {
                                if (room.creatorId.Equals((clConn.socket.RemoteEndPoint as IPEndPoint).Address))
                                {
                                    clConn.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                        new AsyncCallback(OnSend), clientSocket);
                                }
                            }
                        }

                        clientSocket.Close();

                        break;

                    //case Command.Message:

                    //    //Set the text of the message that we will broadcast to all users
                    //    msgToSend.strMessage = msgReceived.strName + ": " + msgReceived.strMessage;
                    //    break;

                    //case Command.List:

                    //    //Send the names of all users in the chat room to the new user
                    //    msgToSend.cmdCommand = Command.List;
                    //    msgToSend.strName = null;
                    //    msgToSend.strMessage = null;

                    //    //Collect the names of the user in the chat room
                    //    foreach (ClientInfo client in clientList)
                    //    {
                    //        //To keep things simple we use asterisk as the marker to separate the user names
                    //        msgToSend.strMessage += client.strName + "*";
                    //    }

                    //    message = msgToSend.ToByte();

                    //    //Send the name of the users in the chat room
                    //    clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                    //            new AsyncCallback(OnSend), clientSocket);
                    //    break;
                }

                //if (msgToSend.cmdCommand != Command.List)   //List messages are not broadcasted
                //{
                //    message = msgToSend.ToByte();

                //    foreach (ClientInfo clientInfo in clientList)
                //    {
                //        if (clientInfo.socket != clientSocket ||
                //            msgToSend.cmdCommand != Command.Login)
                //        {
                //            //Send the message to all users
                //            clientInfo.socket.BeginSend(message, 0, message.Length, SocketFlags.None,
                //                new AsyncCallback(OnSend), clientInfo.socket);
                //        }
                //    }

                //    txtLog.Text += msgToSend.strMessage + "\r\n";
                //}

                ////If the user is logging out then we need not listen from her
                //if (msgReceived.cmdCommand != Command.Logout)
                //{
                //    //Start listening to the message send by the user
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientSocket);
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GUNBOND TRACKER ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnSend(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GUNBOND TRACKER ONSEND ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
