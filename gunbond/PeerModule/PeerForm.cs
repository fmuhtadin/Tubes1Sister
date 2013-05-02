using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using GameEngine;
using System.Threading;

namespace PeerModule
{
    public partial class PeerForm : Form
    {
        public Socket clientSocket; //The main client socket
        public string strName;      //Name by which the user logs into the room

        private byte[] byteData = new byte[1024];

        GunbondPeer peer;
        GunbondGame game;

        public PeerForm()
        {
            InitializeComponent();
            lbRoom.Items.Add("ROOM 1");
            lbRoom.Items.Add("ROOM 2");
            peer = new GunbondPeer(this);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonJ_Click(object sender, EventArgs e)
        {

        }

        private void buttonR_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void connect_Click(object sender, EventArgs e)
        {
            peer.InitSocket(textIPServer.Text);
        }

        public void setMessagesText(String S)
        {
            textMessages.Invoke((MethodInvoker) (() => textMessages.Text = S));
        }

        public void setRoomListBox(List<String> list)
        {
            lbRoom.Invoke((MethodInvoker) (() => lbRoom.Items.Clear()));
            foreach (String s in list)
                lbRoom.Invoke((MethodInvoker) (() => lbRoom.Items.Add(s)));
        }

        public void setPeerIdText(String S)
        {
            textPeerId.Invoke((MethodInvoker)(() => textPeerId.Text = S));
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void createRoom_Click(object sender, EventArgs e)
        {
            peer.CreateRoom(roomName.Text, Convert.ToInt32(textPlayerNum.Text));
        }

        private void buttonR_Click_1(object sender, EventArgs e)
        {
            peer.ListRoom();
        }

        private void PeerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Closing Socket...");
            peer.ClosePeer();
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                game = new GunbondGame();
                game.Run();
            });
            thread.Start();
            thread.Join();
        }
    }
}
