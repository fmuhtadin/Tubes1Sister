using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrackerModule;
using PeerModule;

namespace gunbond
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonTracker_Click(object sender, EventArgs e)
        {
            new TrackerForm().ShowDialog();
        }

        private void buttonPeer_Click(object sender, EventArgs e)
        {
            new PeerForm().ShowDialog();
        }
    }
}
