using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GunbondLibrary;
using System.Net;

namespace TrackerModule
{
    public partial class TrackerForm : Form
    {
        GunbondTracker tracker;

        public TrackerForm()
        {
            InitializeComponent();
            tracker = new GunbondTracker(this);
        }

        private void InitEncoding() {
        }

        public void SetTextMessagesReceived(String s) 
        {
            textMessagesReceived.Invoke((MethodInvoker)(() => textMessagesReceived.Text = s));
        }

        public void SetRoomListBox(List<Room> list) 
        {
            lbListRoom.Invoke((MethodInvoker)(() => lbListRoom.Items.Clear()));
            foreach (Room s in list)
                lbListRoom.Invoke((MethodInvoker)(() => lbListRoom.Items.Add(s.roomId)));
        }

        public void SetPeerListBox(List<ConnectionState> list)
        {
            lbListPeer.Invoke((MethodInvoker)(() => lbListPeer.Items.Clear()));
            foreach (ConnectionState s in list)
                lbListPeer.Invoke((MethodInvoker)(() => lbListPeer.Items.Add((s.socket.RemoteEndPoint as IPEndPoint).Address).ToString()));
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            tracker.InitSocket();
        }

        private void TrackerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Closing Socket...");
            tracker.CloseTracker();
        }
    }
}
