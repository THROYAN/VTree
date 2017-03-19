using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using VTree.Events;
using VTree.Models;

namespace VTreeTest
{
    class Program
    {
        static object scanLock = new object();

        static Queue<ItemFoundEventArgs> q1 = new Queue<ItemFoundEventArgs>();
        static Queue<ItemFoundEventArgs> q2 = new Queue<ItemFoundEventArgs>();

        static void Main(string[] args)
        {
            DirectoryScanner scanner = new DirectoryScanner();

            CreateSubscribers(scanner);
            Thread scanThread = new Thread(() =>
            {
                log("scan start");
                scanner.Scan("C:\\Windows");
                Stop();
                log("scan end");
            })
            { Name = "Scan thread" };

            scanThread.Start();
            Console.Read();
        }

        static void log(string message)
        {
            Console.WriteLine(Thread.CurrentThread.Name + " [" + Thread.CurrentThread.ManagedThreadId + "]: " + message);
        }

        static void CreateSubscribers(DirectoryScanner scanner)
        {
            scanner.onItemFound += (object sender, ItemFoundEventArgs e) =>
            {
                lock (q1)
                {
                    q1.Enqueue(e);
                    Monitor.Pulse(q1);
                }
            };
            scanner.onItemFound += (object sender, ItemFoundEventArgs e) =>
            {
                lock (q2)
                {
                    q2.Enqueue(e);
                    Monitor.Pulse(q2);
                }
            };

            var thread = new Thread(() =>
            {
                log("subscribe");

                while (true)
                {
                    lock (q1)
                    {
                        while (q1.Count == 0)
                        {
                            Monitor.Wait(q1);
                        }
                        ItemFoundEventArgs i = q1.Dequeue();
                        log(i.Info.FullName);
                    }
                }
            });
            thread.Name = "Console output";

            var thread2 = new Thread(() =>
            {
                log("subscribe");

                while (true)
                {
                    lock (q2)
                    {
                        while (q2.Count == 0)
                        {
                            Monitor.Wait(q2);
                        }
                        ItemFoundEventArgs i = q2.Dequeue();
                        log(i.Info.FullName);
                    }
                }
            });
            thread2.Name = "Console output!!!!";

            thread.Start();
            thread2.Start();

            AsyncEventSubscriber<ItemFoundEventArgs> outputToConsole = new AsyncEventSubscriber<ItemFoundEventArgs>();
            outputToConsole.OnEvent += (object sender, ItemFoundEventArgs e) => {
                log(e.Info.FullName);
            };
            scanner.onItemFound += outputToConsole.Handler;
            outputToConsole.Thread.Start();

            // xml
            XmlWriter xmlWriter = XmlWriter.Create("1.xml");
            directoryWriter = new DirectoryXMLWriter(xmlWriter);
            AsyncEventSubscriber<ItemFoundEventArgs> writeToFile = new AsyncEventSubscriber<ItemFoundEventArgs>();
            bool first = true;

            writeToFile.OnEvent += (object sender, ItemFoundEventArgs e) => {
                lock (directoryWriter)
                {
                    if (first)
                    {
                        first = false;
                        directoryWriter.Start();
                    }

                    if (e.Info is DirectoryInfo)
                    {
                        directoryWriter.WriteDirectory(e.Info as DirectoryInfo);
                    }
                    else
                    {
                        directoryWriter.WriteFile(e.Info as FileInfo);
                    }
                }
            };
            scanner.onItemFound += writeToFile.Handler;
            writeToFile.Thread.Start();
        }

        private static DirectoryXMLWriter directoryWriter;

        static void Stop()
        {
            lock (directoryWriter)
            {
                directoryWriter.End();
            }
        }

        public static bool HasFolderWritePermission(string destDir)
        {
            if (string.IsNullOrEmpty(destDir) || !Directory.Exists(destDir)) return false;
            try
            {
                DirectorySecurity security = Directory.GetAccessControl(destDir);
                WindowsIdentity users = WindowsIdentity.GetCurrent();
                foreach (AuthorizationRule rule in security.GetAccessRules(true, true, typeof(SecurityIdentifier)))
                {
                    if (users.User == rule.IdentityReference || users.Groups.Contains(rule.IdentityReference))
                    {
                        FileSystemAccessRule rights = ((FileSystemAccessRule)rule);
                        if (rights.AccessControlType == AccessControlType.Allow)
                        {
                            if (rights.FileSystemRights == (rights.FileSystemRights | FileSystemRights.Modify)) return true;
                        }

                        Console.WriteLine(rule.IdentityReference + ": " + rights.FileSystemRights);
                    }
                    Console.WriteLine(rule.IdentityReference);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
