using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using VTree.Events;
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
            CreateXMLWriter(mainForm, scanner);

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
            Console.WriteLine("Outside treeform-" + Thread.CurrentThread.ManagedThreadId);
            mainForm.onStart += (Events.DirectoryScanEventArgs e) =>
            {
                TreeForm treeForm = new TreeForm(scanner);
                mainForm.BeginInvoke(new Action(() =>
                {
                    Console.WriteLine("mainForm onstart-" + Thread.CurrentThread.ManagedThreadId);
                    treeForm.ShowDialog(mainForm);
                    treeForm.InitializeTree();
                }));
            };
        }

        static void CreateXMLWriter(MainForm mainForm, DirectoryScanner scanner)
        {
            DirectoryXMLWriter directoryWriter = null;
            AsyncEventSubscriber<ItemFoundEventArgs> subscriber = null;
            // todo: move it to separate class
            mainForm.onStart += (DirectoryScanEventArgs e) =>
            {
                XmlWriter xmlWriter = XmlWriter.Create(e.XMLFilePath);
                directoryWriter = new DirectoryXMLWriter(xmlWriter);
                // write first lines
                directoryWriter.Start();

                subscriber = new AsyncEventSubscriber<ItemFoundEventArgs>();
                scanner.onItemFound += subscriber.Handler;
                subscriber.OnEvent += (object sender, ItemFoundEventArgs ev) =>
                {
                    lock (directoryWriter)
                    {
                        if (ev.Info is DirectoryInfo)
                        {
                            directoryWriter.WriteDirectory(ev.Info as DirectoryInfo);
                        }
                        else if (ev.Info is FileInfo)
                        {
                            directoryWriter.WriteFile(ev.Info as FileInfo);
                        }
                    }
                };

                subscriber.Thread.Start();
            };

            mainForm.onFinish += (DirectoryScanEventArgs e) =>
            {
                // to be sure close file after writing
                // are we sure?
                lock (directoryWriter)
                {
                    directoryWriter.End();
                    subscriber.Thread.Abort();
                }
            };
        }
    }
}
