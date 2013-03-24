using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PeerModule
{
    public class Program
    {
        [STAThread]    
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            PeerForm PF = new PeerForm();
            Application.Run(PF);
        }
    }
}
