using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using VTree.Forms;
using VTree.Models;

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

            DirectoryScanner scanner = CreateScanner();
            MainForm mainForm = CreateMainForm(scanner);

            CreateTreeForm(mainForm, scanner);

            Application.Run(mainForm);
        }

        static DirectoryScanner CreateScanner()
        {
            return new DirectoryScanner();
        }

        static MainForm CreateMainForm(DirectoryScanner scanner)
        {

            string directory = Properties.Settings.Default.directoryToScan,
                xmlPath = Properties.Settings.Default.xmlPath;

            MainForm mainForm = new MainForm(scanner, directory, xmlPath);
            // save settings
            mainForm.onStart += (Events.DirectoryScanEventArgs e) =>
            {
                Properties.Settings.Default.directoryToScan = e.DirectoryPath;
                Properties.Settings.Default.xmlPath = e.XMLFilePath;

                Properties.Settings.Default.Save();
            };

            return mainForm;
        }

        static void CreateTreeForm(MainForm mainForm, DirectoryScanner scanner)
        {
            TreeForm treeForm = new TreeForm(scanner);
            Console.WriteLine("Outside treeform-" + Thread.CurrentThread.ManagedThreadId);
            mainForm.onStart += (Events.DirectoryScanEventArgs e) =>
            {
                Console.WriteLine("mainForm onstart-" + Thread.CurrentThread.ManagedThreadId);
                new Thread(() =>
                {
                    treeForm.ShowDialog();
                    treeForm.InitializeTree();
                }).Start();
            };

            // close parent form
            // we don't want to reopen form...
            treeForm.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                if (mainForm.InvokeRequired)
                {
                    mainForm.BeginInvoke(new Action(() =>
                    {
                        mainForm.Close();
                    }));

                    return;
                }

                mainForm.Close();
            };
        }
    }
}
