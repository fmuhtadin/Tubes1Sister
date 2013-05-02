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
        public List<IPAddress> listIPAddress;

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
            listIPAddress = new List<IPAddress>();
        }

        public MessageData(byte[] data)
        {
            reservedBytes = new byte[8];
            listRoom = new List<Room>();
            listIPAddress = new List<IPAddress>();
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
                case 182:
                    next = new byte[4];
                    Array.Copy(data,20,next,0,4);
                    peerId = new IPAddress(next);
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
                    for (int i = 0; i < ipCount; i++)
                    {
                        Array.Copy(data, 24 + i * 4, next, 0, 4);
                        IPAddress tmpIP = new IPAddress(next);
                        listIPAddress.Add(tmpIP);
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
                case 182:
                    retByte.AddRange(peerId.GetAddressBytes());
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
                    for (int i = 0; i < listIPAddress.Count; i++)
                    {
                        retByte.AddRange(listIPAddress[i].GetAddressBytes());
                    }
                    break;
            }
            return retByte.ToArray();
        }
    }
}
