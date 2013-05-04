using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets; 

namespace GunbondLibrary
{
    public class Room
    {
        public String roomId;
        public IPAddress creatorId;

        public Room()
        {
        }

        public Room(String roomid, IPAddress creatorid)
        {
            this.roomId = roomid;
            this.creatorId = creatorid;
        }
    }

    public class MessageData
    {
        //global data
        public String pstr;
        public byte[] reservedBytes;
        public byte code;

        //optional data
        public IPAddress peerId;
        public int maxPlayerNum;
        public int roomCount;
        public String roomId;
        public List<Room> listRoom;
        public int ipCount;
        public int team1Count;
        public int team2Count;
        public List<IPAddress> listIPAddress;
        public List<IPAddress> listIPTeam1;
        public List<IPAddress> listIPTeam2;
        public int state;
        public int packetCounter;
        public List<int> turn;

        public MessageData()
        {
            pstr = "GunbondGame";
            reservedBytes = new byte[8];
            for (int i = 0; i < reservedBytes.Length; i++)
            {
                reservedBytes[i] = 0x00;
            }
            code = 0;
            peerId = null;
            maxPlayerNum = 0;
            roomCount = 0;
            roomId = null;
            listRoom = new List<Room>();
            ipCount = 0;
            team1Count = 0;
            team2Count = 0;
            state = 0;
            packetCounter = 0;
            listIPAddress = new List<IPAddress>();
            listIPTeam1 = new List<IPAddress>();
            listIPTeam2 = new List<IPAddress>();
            turn = new List<int>();
        }

        public MessageData(byte[] data)
        {
            reservedBytes = new byte[8];
            listRoom = new List<Room>();
            listIPAddress = new List<IPAddress>();
            listIPTeam1 = new List<IPAddress>();
            listIPTeam2 = new List<IPAddress>();
            turn = new List<int>();
            this.pstr = Encoding.UTF8.GetString(data, 0, 11);
            for (int i = 0; i<reservedBytes.Length; i++) {
                this.reservedBytes[i] = data[10+i];
            }

            this.code = data[19];

            int mCode = code;

            byte[] next;
            switch (mCode)
            {
                case 135:
                    next = new byte[4];
                    if (!Array.Equals(next, new byte[] {0x00, 0x00, 0x00, 0x00}))
                    {
                        Array.Copy(data, 20, next, 0, 4);
                        peerId = new IPAddress(next);
                    }
                    break;
                case 181:
                    next = new byte[4];
                    Array.Copy(data, 20, next, 0, 4);
                    peerId = new IPAddress(next);
                    packetCounter = BitConverter.ToInt32(data, 24);
                    break;
                case 182:
                    next = new byte[4];
                    Array.Copy(data,20,next,0,4);
                    peerId = new IPAddress(next);
                    packetCounter = BitConverter.ToInt32(data, 24);
                    break;
                case 255:
                    next = new byte[4];
                    Array.Copy(data,20,next,0,4);
                    peerId = new IPAddress(next);
                    this.maxPlayerNum = BitConverter.ToInt32(data, 24);
                    next = new byte[50];
                    Array.Copy(data, 28, next, 0, 50);
                    roomId = Encoding.UTF8.GetString(next, 0, 50);
                    break;
                case 254:
                    next = new byte[4];
                    Array.Copy(data,20,next,0,4);
                    peerId = new IPAddress(next);
                    break;
                case 200:
                    this.roomCount = BitConverter.ToInt32(data, 20);
                    for (int i = 0; i < roomCount; i++)
                    {
                        Room room = new Room();
                        next = new byte[50];
                        Array.Copy(data, 24 + i * 54, next, 0, 50);
                        room.roomId = Encoding.UTF8.GetString(next, 0, 50);
                        next = new byte[4];
                        Array.Copy(data, 74 + i * 54, next, 0, 4);
                        room.creatorId = new IPAddress(next);
                        listRoom.Add(room);
                    }
                    break;
                case 127:
                    break;
                case 128:
                    break;
                case 253:
                    next = new byte[4];
                    Array.Copy(data, 20, next, 0, 4);
                    peerId = new IPAddress(next);
                    next = new byte[50];
                    Array.Copy(data, 24, next, 0, 50);
                    roomId = Encoding.UTF8.GetString(next, 0, 50);
                    break;
                case 252:
                    next = new byte[4];
                    Array.Copy(data, 20, next, 0, 4);
                    peerId = new IPAddress(next);
                    next = new byte[50];
                    Array.Copy(data, 24, next, 0, 50);
                    roomId = Encoding.UTF8.GetString(next, 0, 50);
                    break;
                case 235:
                    next = new byte[4];
                    Array.Copy(data, 20, next, 0, 4);
                    peerId = new IPAddress(next);
                    break;
                case 100:
                    //list IP Address connected to superpeer
                    next = new byte[4];
                    this.ipCount = BitConverter.ToInt32(data, 20);
                    this.team1Count = BitConverter.ToInt32(data, 24);
                    this.team2Count = BitConverter.ToInt32(data, 28);
                    int count = 0;
                    for (int i = 0; i < ipCount; i++)
                    {
                        Array.Copy(data, 32 + count * 4, next, 0, 4);
                        IPAddress tmpIP = new IPAddress(next);
                        listIPAddress.Add(tmpIP);
                        count++;
                    }
                    for (int i = 0; i < team1Count; i++)
                    {
                        Array.Copy(data, 32 + count * 4, next, 0, 4);
                        IPAddress tmpIP = new IPAddress(next);
                        listIPTeam1.Add(tmpIP);
                        count++;
                    }
                    for (int i = 0; i < team2Count; i++)
                    {
                        Array.Copy(data, 32 + count * 4, next, 0, 4);
                        IPAddress tmpIP = new IPAddress(next);
                        listIPTeam2.Add(tmpIP);
                        count++;
                    }
                    break;
                case 70:
                    next = new byte[4];
                    Array.Copy(data, 20, next, 0, 4);
                    peerId = new IPAddress(next);
                    this.state = BitConverter.ToInt32(data, 24);
                    this.packetCounter = BitConverter.ToInt32(data, 28);
                    break;
                case 130:
                    next = new byte[4];
                    this.maxPlayerNum = BitConverter.ToInt32(data, 20);
                    this.packetCounter = BitConverter.ToInt32(data, 24);
                    for (int i = 0; i < maxPlayerNum; i++)
                    {
                        int tmp = BitConverter.ToInt32(data, 28 + i * 4);
                        turn.Add(tmp);
                    }
                    break;
            }

        }

        public byte[] ToByte()
        {
            List<byte> retByte = new List<byte>();

            retByte.AddRange(Encoding.UTF8.GetBytes(pstr));
            retByte.AddRange(reservedBytes);
            retByte.AddRange(new byte[] {code});

            switch (code)
            {
                case 135:
                    if (peerId != null)
                    {
                        retByte.AddRange(peerId.GetAddressBytes());
                    }
                    break;
                case 181:
                    retByte.AddRange(peerId.GetAddressBytes());
                    retByte.AddRange(BitConverter.GetBytes(packetCounter));
                    break;
                case 182:
                    retByte.AddRange(peerId.GetAddressBytes());
                    retByte.AddRange(BitConverter.GetBytes(packetCounter));
                    break;
                case 255:
                    retByte.AddRange(peerId.GetAddressBytes());
                    retByte.AddRange(BitConverter.GetBytes(maxPlayerNum));
                    retByte.AddRange(Encoding.UTF8.GetBytes(roomId));
                    break;
                case 254:
                    retByte.AddRange(peerId.GetAddressBytes());
                    break;
                case 200:
                    retByte.AddRange(BitConverter.GetBytes(roomCount));
                    for (int i = 0; i < listRoom.Count; i++)
                    {
                        byte[] toAdd = new byte[50];
                        Encoding.UTF8.GetBytes(listRoom[i].roomId, 0, 50, toAdd, 0);
                        retByte.AddRange(toAdd);
                        toAdd = new byte[4];
                        toAdd = listRoom[i].creatorId.GetAddressBytes();
                        retByte.AddRange(toAdd);
                    }
                    break;
                case 127:
                    break;
                case 128:
                    break;
                case 253:
                    retByte.AddRange(peerId.GetAddressBytes());
                    retByte.AddRange(Encoding.UTF8.GetBytes(roomId));
                    break;
                case 252:
                    retByte.AddRange(peerId.GetAddressBytes());
                    retByte.AddRange(Encoding.UTF8.GetBytes(roomId));
                    break;
                case 235:
                    retByte.AddRange(peerId.GetAddressBytes());
                    break;
                case 230:
                    break;
                case 100:
                    retByte.AddRange(BitConverter.GetBytes(ipCount));
                    retByte.AddRange(BitConverter.GetBytes(team1Count));
                    retByte.AddRange(BitConverter.GetBytes(team2Count));
                    for (int i = 0; i < listIPAddress.Count; i++)
                    {
                        retByte.AddRange(listIPAddress[i].GetAddressBytes());
                    }
                    for (int i = 0; i < listIPTeam1.Count; i++)
                    {
                        retByte.AddRange(listIPTeam1[i].GetAddressBytes());
                    }
                    for (int i = 0; i < listIPTeam2.Count; i++)
                    {
                        retByte.AddRange(listIPTeam2[i].GetAddressBytes());
                    }
                    break;
                case 70:
                    retByte.AddRange(peerId.GetAddressBytes());
                    retByte.AddRange(BitConverter.GetBytes(state));
                    retByte.AddRange(BitConverter.GetBytes(packetCounter));
                    break;
                case 130:
                    retByte.AddRange(BitConverter.GetBytes(maxPlayerNum));
                    retByte.AddRange(BitConverter.GetBytes(packetCounter));
                    for (int i = 0; i < maxPlayerNum; i++)
                    {
                        retByte.AddRange(BitConverter.GetBytes(turn[i]));
                    }
                    break;
            }
            return retByte.ToArray();
        }
    }
}
