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

            CreateXMLWriter(mainForm, scanner);
            // this one for deep tree in XML
            CreateTreeXMLWriter(mainForm, scanner);

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
            mainForm.onScanStart += (Events.DirectoryScanEventArgs e) =>
            {
                Properties.Settings.Default.directoryToScan = e.DirectoryPath;
                Properties.Settings.Default.xmlPath = e.XMLFilePath;

                Properties.Settings.Default.Save();
            };

            return mainForm;
        }

        static void CreateXMLWriter(MainForm mainForm, DirectoryScanner scanner)
        {
            // to use it in onScanFinish bellow
            DirectoryXMLWriter directoryWriter = null;
            AsyncEventSubscriber<ItemFoundEventArgs> subscriber = null;

            // todo: move it to separate class
            mainForm.onScanStart += (DirectoryScanEventArgs e) =>
            {
                // todo: check second start
                try
                {
                    // todo: move to the lock
                    // so we could be sure that previous file is closed
                    XmlWriter xmlWriter = XmlWriter.Create(e.XMLFilePath);
                    directoryWriter = new DirectoryXMLWriter(xmlWriter);
                }
                catch
                {
                    MessageBox.Show("File is not writable");

                    return;
                }
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
                            directoryWriter.StartDirectory();
                            directoryWriter.WriteDirectory(ev.Info as DirectoryInfo);
                            directoryWriter.EndDirectory();
                        }
                        else if (ev.Info is FileInfo)
                        {
                            directoryWriter.StartFile();
                            directoryWriter.WriteFile(ev.Info as FileInfo);
                            directoryWriter.EndFile();
                        }
                    }
                };

                // start subscriber in new thread
                subscriber.Thread.Start();
            };

            mainForm.onScanFinish += (DirectoryScanEventArgs e) =>
            {
                // last one failed?
                if (directoryWriter == null)
                {
                    return;
                }
                // to be sure close file after writing
                // are we sure?
                lock (directoryWriter)
                {
                    try
                    {
                        subscriber.Thread.Abort();
                        directoryWriter.End();
                    }
                    catch
                    {
                        // can be failed on second run
                    }
                }
            };
        }

        static void CreateTreeXMLWriter(MainForm mainForm, DirectoryScanner scanner)
        {
            DirectoryTree tree = new DirectoryTree();
            AsyncEventSubscriber<ItemFoundEventArgs> subscriber = null;
            // todo: move it to separate class
            mainForm.onScanStart += (DirectoryScanEventArgs e) =>
            {
                tree.Clear();
                // todo: check second start
                subscriber = new AsyncEventSubscriber<ItemFoundEventArgs>();
                scanner.onItemFound += subscriber.Handler;
                subscriber.OnEvent += (object sender, ItemFoundEventArgs ev) =>
                {
                    lock (tree)
                    {
                        if (ev.Info is DirectoryInfo)
                        {
                            tree.AddDirectory(ev.Info as DirectoryInfo);
                        }
                        else if (ev.Info is FileInfo)
                        {
                            tree.AddFile(ev.Info as FileInfo);
                        }
                    }
                };

                subscriber.Thread.Start();
            };

            mainForm.onScanFinish += (DirectoryScanEventArgs e) =>
            {
                lock (tree)
                {
                    // it hope it's not null
                    subscriber.Thread.Abort();

                    // write XML in brand new thread
                    new Thread(() =>
                    {
                        // big one
                        try
                        {
                            XmlWriter xmlWriter = XmlWriter.Create(e.XMLFilePath + "-deep.xml");
                            DirectoryXMLWriter directoryWriter = new DirectoryXMLWriter(xmlWriter);
                            
                            Action<DirectoryModel> writeDirFunc = null;
                            writeDirFunc = (DirectoryModel directory) =>
                            {
                                directoryWriter.StartDirectory();
                                directoryWriter.WriteDirectory(directory.Info);
                                // am I lazy?
                                // I cannot understand the domain
                                // I cannot really come up with some place I can move this shit to
                                // should I create one more "DirectoryXMLWriter" or maybe replace that one with this?
                                xmlWriter.WriteElementString("size", directory.Size + " bytes");
                                xmlWriter.WriteStartElement("directories");
                                directory.Directories.ForEach(writeDirFunc);
                                xmlWriter.WriteEndElement();
                                xmlWriter.WriteStartElement("files");
                                directory.Files.ForEach((FileInfo file) =>
                                {
                                    directoryWriter.StartFile();
                                    directoryWriter.WriteFile(file);
                                    directoryWriter.EndFile();
                                });
                                xmlWriter.WriteEndElement();
                                directoryWriter.EndDirectory();
                            };

                            directoryWriter.Start();
                            writeDirFunc(tree.GetRoot());
                            directoryWriter.End();
                        }
                        catch
                        {
                            MessageBox.Show("File is not writable");

                            return;
                        }
                    }).Start();
                }
            };
        }
    }
}
