using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using VTree.Events;

namespace VTree.Models
{
    public class DirectoryScanner
    {
        public delegate void FileFoundHandler(FileFoundEventArgs e);
        public delegate void DirectoryFoundHandler(DirectoryFoundEventArgs e);
        public delegate void ErrorHandler(ErrorEventArgs e);

        public event FileFoundHandler onFileFound;
        public event DirectoryFoundHandler onDirectoryFound;
        public event ErrorHandler onError;

        public DirectoryScanner()
        {
        }

        /// <summary>
        /// Scans directory recursively
        /// </summary>
        /// <param name="path">path to scan</param>
        public void Scan(string path)
        {
            this.onDirectoryFound?.Invoke(new DirectoryFoundEventArgs(
                new DirectoryInfo(path)
            ));

            this.scan(path);
        }

        private void scan(string path)
        {
            string[] files;
            try
            {
                files = Directory.GetFiles(path);
            }
            catch (Exception e)
            {
                this.onError?.Invoke(new ErrorEventArgs(e));

                return;
            }

            foreach (string file in Directory.GetFiles(path))
            {
                this.onFileFound?.Invoke(new FileFoundEventArgs(
                    new FileInfo(file)
                ));
            }

            string[] directories = Directory.GetDirectories(path);

            // two cycles to show all directories first
            foreach (string dir in directories)
            {
                this.onDirectoryFound?.Invoke(new DirectoryFoundEventArgs(
                    new DirectoryInfo(dir)
                ));
            }
            // search forward
            foreach (string dir in directories)
            {
                this.scan(dir);
            }
        }
    }
}
