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
            MainForm mainForm = new MainForm(scanner);
            CreateTreeForm(mainForm, scanner);

            Application.Run(mainForm);
        }

        static DirectoryScanner CreateScanner()
        {
            return new DirectoryScanner();
        }

        static void CreateTreeForm(MainForm mainForm, DirectoryScanner scanner)
        {
            Console.WriteLine("Outside treeform-" + Thread.CurrentThread.ManagedThreadId);
            new Thread(new ThreadStart(() => {
                TreeForm treeForm = new TreeForm(scanner);
                Console.WriteLine("Treeform creation-" + Thread.CurrentThread.ManagedThreadId);
                mainForm.onStart += (Events.DirectoryScanEventArgs e) =>
                {
                    treeForm.Show();
                    Console.WriteLine("mainForm onstart-" + Thread.CurrentThread.ManagedThreadId);
                    treeForm.BeginInvoke(new Action(() =>
                    {
                        treeForm.initializeTree();
                        Console.WriteLine("treeform invoke-" + Thread.CurrentThread.ManagedThreadId);
                    }));
                };
            })).Start();
        }
    }
}
