using System;
using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;

namespace Import
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LoginDlg Logdlg = new LoginDlg();
            Logdlg.ServerName.Text = Properties.Settings1.Default.ServerName;
            Logdlg.pdbUserName.Text = Properties.Settings1.Default.PowerName;
            Logdlg.pdbPassword.Text = Properties.Settings1.Default.PowerPassword;
            Logdlg.ShowDialog();
            if (Logdlg.bRun)
            {
                Application.Run(new MainForm(Logdlg));                
            }
        }
    }
}
