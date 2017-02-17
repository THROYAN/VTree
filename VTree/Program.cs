using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace VTree
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
            // todo: create all threads here
            // in main form we only start thread with scanning
            // and thread with tree
            // and thread with xml
            Application.Run(new MainForm());
        }
    }
}
