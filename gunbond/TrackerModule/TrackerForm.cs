using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

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
