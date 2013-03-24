using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrackerModule
{
    public partial class TrackerForm : Form
    {
        GunbondTracker tracker;

        public TrackerForm()
        {
            InitializeComponent();
            tracker = new GunbondTracker();
        }
    }
}
