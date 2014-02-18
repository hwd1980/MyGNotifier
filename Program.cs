using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyGNotifier
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

            if (MyGNotifier.Properties.Settings.Default.MAIN_PASSWORD != "")
            {
                if (MyGNotifier.Properties.Settings.Default.MAIN_PASS_SHOW)
                {
                    Application.Run(new MGN_LOGIN());
                }
                else
                {
                    Application.Run(new MGN_IMPOSTAZIONI());
                }
            }
            else
            {
                Application.Run(new MGN_SET_PASSWORD());
            }
            
        }
    }
}
